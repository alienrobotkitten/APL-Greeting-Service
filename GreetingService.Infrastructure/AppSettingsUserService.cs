using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

/// <summary>
/// Summary description for Class1
/// </summary>
public class AppSettingsUserService : IUserService
{
    private IConfiguration _config;
    private ILogger<AppSettingsUserService> _logger;

    public AppSettingsUserService(IConfiguration config, ILogger<AppSettingsUserService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public bool IsValidUser(string username, string password)
    {
        var entries = _config.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
        if (entries.TryGetValue(username, out var storedPassword))
        {
            _logger.LogInformation($"Valid credentials for user {username}.");
            return storedPassword == password;
        }

        _logger.LogWarning($"User {username} did not provide valid credentials.");
        return false;
    }
}
