using Azure.Messaging.ServiceBus;
using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GreetingService.Infrastructure.MessagingServices;
public class ServiceBusMessagingService : IMessagingService
{
    private readonly IConfiguration _config;

    private ServiceBusClient _serviceBusClient { get; set; }
    public ServiceBusMessagingService(IConfiguration config, ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
        _config = config;
    }
    public async Task SendAsync<T>(T message, string subject)
    {
        string jsonMessage = JsonSerializer.Serialize<T>(message);
        ServiceBusMessage sbm = new ServiceBusMessage(jsonMessage);
        sbm.Subject = subject;
      
        ServiceBusSender sender = _serviceBusClient.CreateSender("main");
        await sender.SendMessageAsync(sbm);

        //// create a receiver that we can use to receive the message
        //ServiceBusReceiver receiver = client.CreateReceiver(queueName);

        //// the received message is a different type as it contains some service set properties
        //ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

        //// get the message body as a string
        //string body = receivedMessage.Body.ToString();
        //Console.WriteLine(body);
    }
}
