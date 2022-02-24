using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure.UserServices;

public class SqlUserService : IUserServiceAsync
{
    private readonly IConfiguration _config;
    private readonly ILogger<SqlUserService> _logger;
    private readonly GreetingDbContext _dbContext;
    private readonly string _containerName = "users";
    private const string _blobName = "user.json";
    private readonly string _connectionString;
    private readonly BlobContainerClient _userBlobContainerClient;
    private BlobClient _userBlobClient;
    private Dictionary<string, string> _users;

    public SqlUserService(IConfiguration config, ILogger<SqlUserService> logger, GreetingDbContext dbContext)
    {
        _config = config;
        _logger = logger;
        _dbContext = dbContext;
        _connectionString = _config["GreetingDbConnectionString"];
    }

    public async Task<bool> IsValidUserAsync(string username, string password)
    {
        var user = await _dbContext.Users.FindAsync(username);

        if (user != null)
        {
            return user.Password == password;
        }
        else
        {
            _logger.LogWarning($"User '{username}' not found.");
            return false;
        }
    }
}
