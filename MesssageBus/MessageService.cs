using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace MesssageClient
{
    public class MessageService : IMessageService
    {
        private IModel Channel { set; get; }
        private IConnection Connection { set; get; } 
        private MessageModel Message { set; get; }

        private readonly IAppSettings _appSettings;

        public MessageService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            CreateConnection();
        }

        private void CreateConnection()
        {
            

            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = _appSettings.GetHostName(),
                UserName = _appSettings.GetUsername(),
                Password = _appSettings.GetPassword(),
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = 30,
                Port = 5672
            };// goes to web config
            Connection = connectionFactory.CreateConnection();
            Channel = Connection.CreateModel();
        }
        private void DisposeConnection()
        {
            Connection.Dispose();
            Channel.Dispose();
        }

        public void Subscribe(MessageType messageType)
        {
            Message = new MessageModel { Type = messageType };
            QueueDeclare();
            Consume();
        }

        public void Send(MessageModel messageModel)
        {
            Message = messageModel;
            QueueDeclare();
            var body = Encoding.UTF8.GetBytes(string.Format(messageModel.Message, messageModel.Name));
            Channel.BasicPublish(exchange: string.Empty, routingKey: GetKeyByMessageType(), basicProperties: null, body: body);
        }

        public void QueueDeclare()
        {
            Channel.QueueDeclare(queue: GetKeyByMessageType(), durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void Consume()
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += ReceivedRequestEvent;
            Channel.BasicConsume(queue: GetKeyByMessageType(), autoAck: true, consumer: consumer);
        }

        private void ReceivedRequestEvent(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var receivedMessage = Encoding.UTF8.GetString(body);
            Console.WriteLine(receivedMessage);
            string name = receivedMessage.Substring(receivedMessage.IndexOf(",") + 1);
            if (Message.Type == MessageType.Message)
            {
                var message = new MessageModel
                {
                    Message = "Hello {0}, I'm your father",
                    Name = name,
                    Type = MessageType.Response
                };
                //send response to sender
                Send(message);
            }
            else
            {
                //if is reponse has been sent back to the senderapp, close connection
                DisposeConnection();
            }

        }

        private string GetKeyByMessageType()
        {
            return Message.Type == MessageType.Message ? _appSettings.GetSentMessageKey() : _appSettings.GetReceivedMessageKey();
        }
    }
}
