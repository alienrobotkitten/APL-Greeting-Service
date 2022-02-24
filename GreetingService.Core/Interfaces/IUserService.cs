namespace GreetingService.Core.Interfaces;

public interface IUserService
{
    public bool IsValidUser(string username, string password);
}
