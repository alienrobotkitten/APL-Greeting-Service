using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure.UserServices;

public class AppSettingsUserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly ILogger<AppSettingsUserService> _logger;

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

            return storedPassword == password;
        }

        _logger.LogWarning($"User '{username}' not found.");
        return false;
    }
}
