namespace GreetingService.Core.Interfaces;
public interface IMessagingService
{
    Task SendAsync<T>(T message, string subject);
}
