using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GreetingService.Infrastructure.GreetingRepositories;

public class CosmosDbGreetingRepository : IGreetingRepositoryAsync
{
    private CosmosClient cosmosClient;

    private Database database;
    private string databaseId = "greetingdb";

    private Container greetings;
    private ILogger<CosmosDbGreetingRepository> _log;
    private string containerId = "Greetings";

    public CosmosDbGreetingRepository(IConfiguration config, ILogger<CosmosDbGreetingRepository> log)
    {
        cosmosClient = new CosmosClient(config["CosmosDbConnectionString"]);
        database = cosmosClient.GetDatabase(databaseId);
        greetings = database.GetContainer(containerId);
        _log = log;

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

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            Greeting? gr = (from g in greetings.GetItemLinqQueryable<Greeting>(allowSynchronousQueryExecution: true)
                            where g.Id == id
                            select g).FirstOrDefault();
            greetings.DeleteItemAsync<Greeting>(id.ToString(), new PartitionKey(gr.From));
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError("No greeting with id " + id.ToString(), ex);
            return false;
        }
    }

    public async Task<Greeting>? GetAsync(Guid id)
    {
        try
        {
            var gr = (from g in greetings.GetItemLinqQueryable<Greeting>(allowSynchronousQueryExecution: true) 
                           where g.Id == id 
                           select g)
                           .FirstOrDefault();
            return gr;
        }
        catch (Exception ex)
        {
            _log.LogError("No greeting with id " + id.ToString(), ex);
            return null;
        }
    }

    public async Task<IEnumerable<Greeting>> GetAsync()
    {
        IEnumerable<Greeting> gr = (from g in greetings.GetItemLinqQueryable<Greeting>(allowSynchronousQueryExecution: true) select g).AsEnumerable();
        return gr;
    }

    public async Task<IEnumerable<Greeting>> GetAsync(string fromUser = null, string toUser = null)
    {
        List<Greeting> result;
        var gr = (from g in greetings.GetItemLinqQueryable<Greeting>() 
                  select g);

        if (toUser != null)
            toUser = toUser.Trim().ToLower();

        if (fromUser != null)
            fromUser = fromUser.Trim().ToLower();

        if (fromUser != null && toUser != null)
        {
            var query = from g in gr
                        where g.To == toUser && g.From == fromUser
                        select g;
            query.FirstOrDefault();
            result = query.ToList<Greeting>();

        }
        else if (fromUser != null && toUser == null)
        {
            var query = from g in gr
                        where g.From == fromUser
                        select g;
            result = query.ToList();
        }
        else if (fromUser == null && toUser != null)
        {
            var query = from g in gr
                        where g.To == toUser
                        select g;
            result = query.ToList();
        }
        else
        {
            result = gr.ToList<Greeting>();
        }
        return result;
    }

    public async Task<bool> UpdateAsync(Greeting g)
    {
        Greeting? oldGreeting = (from gr in greetings.GetItemLinqQueryable<Greeting>(allowSynchronousQueryExecution: true)
                                 where gr.Id == g.Id
                                 select gr).FirstOrDefault();
        if (oldGreeting != null)
        {
            await greetings.UpsertItemAsync(g);
            return true;
        }
        else
        {
            return false;
        }
    }
}
