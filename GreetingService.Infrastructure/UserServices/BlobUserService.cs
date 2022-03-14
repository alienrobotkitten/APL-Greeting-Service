using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure.UserServices;

public class BlobUserService : IUserServiceAsync
{
    private readonly IConfiguration _config;
    private readonly ILogger<BlobUserService> _logger;
    private readonly string _containerName = "users";
    private const string _blobName = "user.json";
    private readonly string _connectionString;
    private readonly BlobContainerClient _userBlobContainerClient;
    private BlobClient _userBlobClient;
    private Dictionary<string, string> _users;

    public BlobUserService(IConfiguration config, ILogger<BlobUserService> logger)
    {
        _config = config;
        _logger = logger;

        _connectionString = _config["LoggingStorageAccount"];

        _userBlobContainerClient = new(_connectionString, _containerName);
    }

    private async Task InitUserDatabaseAsync()
    {
        _users = new Dictionary<string, string>() {
                { "correct", "horse" },
                { "battery", "staple" }
            };
        BinaryData userJson = new(_users, GreetingExtensions._serializerOptions);

        _userBlobClient = _userBlobContainerClient.GetBlobClient(_blobName);
        await _userBlobClient.UploadAsync(userJson);

        _logger.LogInformation("User.json was uploaded to blob storage.");
    }

    public async Task<bool> IsValidUserAsync(string username, string password)
    {
        if (!_userBlobContainerClient.Exists())
        {
            await InitUserDatabaseAsync();
        }
        await foreach (var blob in _userBlobContainerClient.GetBlobsAsync())
        {
            _userBlobClient = _userBlobContainerClient.GetBlobClient(blob.Name);

            var blobContent = await _userBlobClient.DownloadContentAsync();
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
        return false;
    }

    public Task<bool> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task ApproveUserAsync(Guid approvalCode)
    {
        throw new NotImplementedException();
    }

    public Task RejectUserAsync(Guid approvalCode)
    {
        throw new NotImplementedException();
    }
}
