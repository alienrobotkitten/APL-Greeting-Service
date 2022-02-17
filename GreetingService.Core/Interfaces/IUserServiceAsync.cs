namespace GreetingService.Core.Interfaces;

public interface IUserServiceAsync
{
    Task<bool> IsValidUser(string username, string password);
}
