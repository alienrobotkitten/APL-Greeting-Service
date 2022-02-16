using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GreetingService.Infrastructure;
public class BlobGreetingRepository : IGreetingRepositoryAsync
{
    private IConfiguration _config;
    private ILogger<BlobGreetingRepository> _logger;
    private BlobContainerClient _greetingBlobStore;
    private JsonSerializerOptions? _jsonSerializerOptions;
    private readonly string _connectionString;
    private readonly string _containerName;

    public BlobGreetingRepository(ILogger<BlobGreetingRepository> logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
        _jsonSerializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        _connectionString = _config["GreetingService.API.Function:LoggingStorageAccount"];
        _containerName = "greetings";
        _greetingBlobStore = new BlobContainerClient(_connectionString, _containerName);

        _greetingBlobStore.CreateIfNotExistsAsync();
    }

    /// <summary>
    /// Gets all greetings.
    /// </summary>
    /// <returns>List<Greeting></returns>
    public async Task<IEnumerable<Greeting>> GetAsync()
    {
        List<Greeting> allGreetings = new();

        _logger.LogInformation("Attempting to get all greetings...");

        await foreach (BlobItem blob in _greetingBlobStore.GetBlobsAsync())
        {
            BlobClient blobClient = _greetingBlobStore.GetBlobClient(blob.Name);
            Azure.Response<BlobDownloadResult> blobContent = await blobClient.DownloadContentAsync();
            Greeting g = blobContent.Value.Content.ToObjectFromJson<Greeting>();
            allGreetings.Add(g);
        }

        _logger.LogInformation($"Retrieved ${allGreetings.Count} greetings.");
        return allGreetings;
    }

    /// <summary>
    /// Returns greeting with specified id if it exists.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Greeting or null.</returns>
    public async Task<Greeting>? GetAsync(Guid id)
    {
        var blobClient = _greetingBlobStore.GetBlobClient(id.ToString());
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
    }

    /// <summary>
    /// Creates greeting and returns true if it succeded.
    /// </summary>
    /// <param name="greeting"></param>
    /// <returns></returns>
    public async Task<bool> CreateAsync(Greeting g)
    {
        var blobClient = _greetingBlobStore.GetBlobClient(g.Id.ToString());
        if (await blobClient.ExistsAsync())
        {
            _logger.LogWarning("Id already exists.");
            return false;
        }
        else
        {
            blobClient = _greetingBlobStore.GetBlobClient(g.Id.ToString());
            var greetingBinary = new BinaryData(g, _jsonSerializerOptions);
            await blobClient.UploadAsync(greetingBinary);
            _logger.LogInformation($"Added new greeting with id {g.Id} to database.");
            return true;
        }
    }

    /// <summary>
    /// Tries to update Greeting and returns true if succeeded.
    /// </summary>
    /// <param name="updatedGreeting"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(Greeting updatedGreeting)
    {
        var blobClient = _greetingBlobStore.GetBlobClient(updatedGreeting.Id.ToString());

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            var greetingBinary = new BinaryData(updatedGreeting, _jsonSerializerOptions);
            await blobClient.UploadAsync(greetingBinary);

            _logger.LogInformation($"Updated greeting {updatedGreeting.Id}.");

            return true;
        }
        else
        {
            _logger.LogWarning("No such id.");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var blobClient = _greetingBlobStore.GetBlobClient(id.ToString());

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            _logger.LogInformation($"Deleted greeting {id}.");
            return true;
        }
        else
        {
            _logger.LogWarning("No such id");
            return false;
        }
    }
}
