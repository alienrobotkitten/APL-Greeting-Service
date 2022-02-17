using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure;

public class BlobUserService : IUserServiceAsync
{
    private readonly IConfiguration _config;
    private readonly ILogger<BlobUserService> _logger;
    private readonly string _connectionString;
    private readonly string _containerName = "users";
    private BlobContainerClient _userBlobContainerClient;
    private const string _blobName = "user.json";
    private BlobClient _userBlobClient;
    private Dictionary<string, string> _users;

    public BlobUserService(IConfiguration config, ILogger<BlobUserService> logger)
    {
        _config = config;
        _logger = logger;

        _connectionString = _config["LoggingStorageAccount"];
    }

    public async Task<bool> IsValidUser(string username, string password)
    {
        _userBlobContainerClient = new BlobContainerClient(_connectionString, _containerName);

        if (await _userBlobContainerClient.ExistsAsync())
        {
            _userBlobClient = _userBlobContainerClient.GetBlobClient(_blobName);

            if (await _userBlobClient.ExistsAsync())
            {
                Azure.Response<BlobDownloadResult> blobContent = await _userBlobClient.DownloadContentAsync();
                _users = blobContent.Value.Content.ToObjectFromJson<Dictionary<string, string>>();

                if (_users.TryGetValue(username, out var storedPassword))
                {
                    return storedPassword == password;
                }
                else
                {
                    _logger.LogWarning($"User '{username}' not found.");
                    return false;
                }
            }
            else
            {
                _logger.LogError("There is no blob with user information.");
                return false;
            }
        }
        else
        {
            _logger.LogError("There is no container with user information blobs.");
            return false;
        }
    }
}
