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
        _config = config;
        _serviceBusClient = new(config["ServiceBusConnectionString"]);
    }
    public async Task SendAsync<T>(T message, string subject)
    {
        string jsonMessage = JsonSerializer.Serialize<T>(message);
        ServiceBusMessage sbm = new ServiceBusMessage(jsonMessage);
        sbm.Subject = subject;
      
        ServiceBusSender sender = _serviceBusClient.CreateSender("main");
        await sender.SendMessageAsync(sbm);
    }
}
