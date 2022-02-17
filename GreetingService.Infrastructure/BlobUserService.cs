using Azure.Storage.Blobs;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure;

public class BlobUserService : IUserService
{
    private const string _blobName = "user.json";
    private readonly IConfiguration _config;
    private readonly ILogger<BlobUserService> _logger;
    private readonly string _connectionString;
    private readonly BlobContainerClient _userBlobContainerClient;
    private readonly string _containerName;

    public BlobUserService(IConfiguration config, ILogger<BlobUserService> logger)
    {
        _config = config;
        _logger = logger;
        _connectionString = _config["LoggingStorageAccount"];
        _containerName = "users";
        _userBlobContainerClient = new BlobContainerClient(_connectionString, _containerName);

        _userBlobContainerClient.CreateIfNotExistsAsync();
        _userBlobClient = _userBlobContainerClient.GetBlobClient(_blobName);
    }

    public bool IsValidUser(string username, string password)
    {
        var blobClient = _userBlobClient.
        if (await blobClient.ExistsAsync())
        {
            Azure.Response<BlobDownloadResult> blobContent = await blobClient.DownloadContentAsync();
            Greeting g = blobContent.Value.Content.ToObjectFromJson<Greeting>();
            _logger.LogInformation($"Successfully retrieved greeting with id: {id}");
            return g;
        }
        else
        {
            _logger.LogWarning($"greeting with id: {id} was not found in database.");
            return null;
        }

        var entries = _config.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
        if (entries.TryGetValue(username, out var storedPassword))
        {
            
            return storedPassword == password;
        }

        _logger.LogWarning($"User '{username}' not found.");
        return false;
    }
}
