namespace MesssageClient
{
    public interface IMessageService
    {
        void Subscribe(MessageType messageType);

        void Send(MessageModel message);
    }
}
