namespace GreetingService.Core.Interfaces;

public interface IUserServiceAsync
{
    Task<bool> IsValidUserAsync(string username, string password);
}
