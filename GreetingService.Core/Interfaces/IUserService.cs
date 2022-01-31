namespace GreetingService.Core.Interfaces;

public interface IUserService
{
    bool IsValidUser(string username, string password);
}
