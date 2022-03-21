using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace GreetingService.Infrastructure.GreetingRepositories;

public class CosmosDbGreetingRepository : IGreetingRepositoryAsync
{

    // ADD THIS PART TO YOUR CODE 
    // The Azure Cosmos DB endpoint for running this sample.
    private static readonly string EndpointUri = "https://helena-cosmosdb-dev.documents.azure.com:443/";
    // The primary key for the Azure Cosmos account.
    private static readonly string PrimaryKey = "FDabmGqArzXlwlmRf5qw1ZESiJbUVsRwIrxhqe9dOANuAKtBmThoopN5a8rBNMm77OdL1FI2CB3I7gmYdwsjPA==";

    // The Cosmos client instance
    private CosmosClient cosmosClient;

    // The database we will create
    private Database database;

    // The container we will create.
    private Container greetings;

    // The name of the database and container we will create
    private string databaseId = "greetingdb";
    private string containerId = "Greetings";
    public CosmosDbGreetingRepository(IConfiguration config)
    {
        cosmosClient = new CosmosClient(config["CosmosDbConnectionString"]);
        database = cosmosClient.GetDatabase(databaseId);
        greetings = database.GetContainer(containerId);

    }
    public async Task<bool> CreateAsync(Greeting g)
    {
        PartitionKey partitionKey = new(g.From);
        try
        {
            // Read the item to see if it exists.  

            ItemResponse<Greeting> existingGreeting = await greetings.ReadItemAsync<Greeting>(g.Id.ToString(), partitionKey);
            
            Console.WriteLine($"Item in database with id: {existingGreeting.Resource.Id} already exists\n");
            return false;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
            ItemResponse<Greeting> greetingResponse = await greetings.CreateItemAsync<Greeting>(g, partitionKey);

            return true;
        }
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Greeting>? GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Greeting>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Greeting>> GetAsync(string from = null, string to = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Greeting g)
    {
        throw new NotImplementedException();
    }
}
