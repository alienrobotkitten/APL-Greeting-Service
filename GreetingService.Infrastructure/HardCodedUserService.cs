using GreetingService.Core.Interfaces;

namespace GreetingService.Infrastructure;

public class HardCodedUserService : IUserService
{
    private readonly Dictionary<string, string> _users = new Dictionary<string, string>()
    {
        {"foo", "bar" },
        {"correct","horse" },
        {"battery","staple" },
        {"abc","123" }
    };
    public bool IsValidUser(string username, string password)
    {
        if (!string.IsNullOrEmpty(username)
            && !string.IsNullOrEmpty(password)
            && _users.ContainsKey(username))
        {
            return (_users[username] == password);
        }
        else
        {
            return false;
        }
    }
}

