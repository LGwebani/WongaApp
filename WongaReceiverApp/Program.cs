using System;
using MesssageClient;
using Microsoft.Extensions.DependencyInjection;

namespace WongaReceiverApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = GetServiceProvider();
            Console.WriteLine("RECEIVER APP");
            var messageService = serviceProvider.GetService<IMessageService>();
            messageService.Subscribe(MessageType.Message);
            Console.ReadLine();
        }
        static ServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                  //.AddSingleton<IAppSettings>(c => new AppSettings())
                  //.AddSingleton<IMessageService>(c => new MessageService(new AppSettings()))
                .AddTransient<IAppSettings, AppSettings>()
                .AddTransient<IMessageService, MessageService>()
                .BuildServiceProvider();
        }
    }
}
