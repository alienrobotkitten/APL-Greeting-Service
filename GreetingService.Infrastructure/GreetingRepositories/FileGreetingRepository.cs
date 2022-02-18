using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GreetingService.Infrastructure.GreetingRepositories;
public class FileGreetingRepository : IGreetingRepository
{
    private readonly string _filename;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly List<Greeting> _greetingDatabase;
    private readonly ILogger<FileGreetingRepository> _logger;

    public FileGreetingRepository(IConfiguration config, ILogger<FileGreetingRepository> logger)
    {
        _logger = logger;

        _serializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        string configname = config["GreetingRepositoryPath"];
        _filename = $"{configname}{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}.json";

        if (File.Exists(_filename))
            File.Delete(_filename);

        _greetingDatabase = ReadDatabaseFromFile();
    }

    /// <summary>
    /// Gets all greetings.
    /// </summary>
    /// <returns>List<Greeting></returns>
    public IEnumerable<Greeting> Get()
    {
        _logger.LogInformation("Retrieved all greetings.");
        return _greetingDatabase;
    }

    /// <summary>
    /// Returns greeting with specified id if it exists.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Greeting or null.</returns>
    public Greeting? Get(Guid id)
    {
        _logger.LogInformation($"Trying to get greeting with id: {id} from database...");
        return GetGreeting(id);
    }

    /// <summary>
    /// Creates greeting and returns true if it succeded.
    /// </summary>
    /// <param name="greeting"></param>
    /// <returns></returns>
    public bool Create(Greeting greeting)
    {
        if (GetGreeting(greeting.Id) != null)
        {
            _logger.LogWarning("Id already exists.");
            return false;
        }
        else
        {
            _greetingDatabase.Add(greeting);
            _logger.LogInformation($"Added new greeting with id {greeting.Id} to database.");
            WriteDatabaseToFile();
            return true;
        }
    }

    /// <summary>
    /// Tries to update Greeting and returns true if succeeded.
    /// </summary>
    /// <param name="updatedGreeting"></param>
    /// <returns></returns>
    public bool Update(Greeting updatedGreeting)
    {
        Greeting? g = GetGreeting(updatedGreeting.Id);

        if (g == null)
        {
            _logger.LogWarning("No such id.");
            return false;
        }
        else
        {
            _greetingDatabase.Remove(g);
            _greetingDatabase.Add(updatedGreeting);
            _logger.LogInformation($"Updated greeting {updatedGreeting.Id}.");
            WriteDatabaseToFile();
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        Greeting? g = GetGreeting(id);

        if (g == null)
        {
            _logger.LogWarning("No such id");
            return false;
        }
        else
        {
            _greetingDatabase.Remove(g);
            _logger.LogInformation($"Deleted greeting {id}.");
            WriteDatabaseToFile();
            return true;
        }
    }




    // private methods
    private Greeting? GetGreeting(Guid id)
    {
        IEnumerable<Greeting> greetings = from g in _greetingDatabase
                                          where g.Id == id
                                          select g;
        return greetings.FirstOrDefault();
    }

    private void WriteDatabaseToFile()
    {
        File.WriteAllText(_filename, JsonSerializer.Serialize(_greetingDatabase, _serializerOptions));
        _logger.LogInformation("Wrote database to file.");
    }
    private List<Greeting> ReadDatabaseFromFile()
    {
        try
        {
            List<Greeting>? database = JsonSerializer.Deserialize<List<Greeting>>(File.ReadAllText(_filename), _serializerOptions) ?? throw new Exception();
            _logger.LogInformation("Successfully read database from file.");
            return database;
        }
        catch (Exception e)
        {
            _logger.LogError($"Couldn't read database from file: {e}");
            return new List<Greeting>();
        }
    }

}
