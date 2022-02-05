using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class AppSettingsUserService : IUserService
{
    private IConfiguration _config;
    
    public AppSettingsUserService(IConfiguration config)
    {
        _config = config;
    }

    public bool IsValidUser(string username, string password)
    {
        var entries = _config.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
        if (entries.TryGetValue(username, out var storedPassword))
            return storedPassword == password;

        return false;
    }
}
