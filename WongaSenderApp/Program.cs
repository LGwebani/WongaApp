using System;
using MesssageClient;
using Microsoft.Extensions.DependencyInjection;

namespace WongaSenderApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("SENDER APP");
            Console.WriteLine("Please enter your name");
            var message = new MessageModel
            {
                Message = "Hello my name is, {0}",
                Name = Console.ReadLine(),
                Type = MessageType.Message
            };

            var serviceProvider = GetServiceProvider();
            var messageService = serviceProvider.GetService<IMessageService>();
            messageService.Send(message);
            messageService.Subscribe(MessageType.Response);
            Console.ReadLine();
        }
        
        static ServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddTransient<IAppSettings, AppSettings>()
                .AddTransient<IMessageService, MessageService>()
                .BuildServiceProvider();
        }
    }
}
