using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;

using Serilog;
using System.Text.Json;

namespace GreetingService.Infrastructure;
public class FileGreetingRepository : IGreetingRepository
{
    private string _filename;
    private IConfiguration _config;
    private string _logfilepath;
    private JsonSerializerOptions _serializerOptions;
    private List<Greeting> _greetingDatabase;
    private Serilog.ILogger _logger;

    public FileGreetingRepository(IConfiguration config)
    {
        _config = config;

        _filename = config["GreetingRepositoryPath"] ?? "./TestGreetings.json";

        _serializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        _logfilepath = config["RepositoryLogPath"] ?? "./greetingrepo.log";

        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(_logfilepath)
            .CreateLogger();

        _greetingDatabase = ReadDatabaseFromFile();
    }

    public FileGreetingRepository(string filename)
    {
        _filename = filename;

        _serializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        _logfilepath = "./greetingrepo.log";

        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(_logfilepath)
            .CreateLogger();

        _greetingDatabase = ReadDatabaseFromFile();
    }

    /// <summary>
    /// Gets all greetings.
    /// </summary>
    /// <returns>List<Greeting></returns>
    public IEnumerable<Greeting> Get()
    {
        _logger.Information("Retrieved all greetings.");
        return _greetingDatabase;
    }

    /// <summary>
    /// Returns greeting with specified id if it exists.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Greeting or null.</returns>
    public Greeting? Get(Guid id)
    {
        _logger.Information($"Trying to get greeting with id: {id} from database...");
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
            _logger.Warning("Id already exists.");
            return false;
        }
        else
        {
            _greetingDatabase.Add(greeting);
            _logger.Information($"Added new greeting with id {greeting.Id} to database.");
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
            _logger.Warning("No such id.");
            return false;
        }
        else
        {
            _greetingDatabase.Remove(g);
            _greetingDatabase.Add(updatedGreeting);
            _logger.Information($"Updated greeting {updatedGreeting.Id}.");
            WriteDatabaseToFile();
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        Greeting? g = GetGreeting(id);

        if (g == null)
        {
            _logger.Warning("No such id");
            return false;
        } 
        else
        {
            _greetingDatabase.Remove(g);
            _logger.Information($"Deleted greeting {id}.");
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
        File.WriteAllText(_filename, JsonSerializer.Serialize<List<Greeting>>(_greetingDatabase, _serializerOptions));
        _logger.Information("Wrote database to file.");
    }
    private List<Greeting> ReadDatabaseFromFile()
    {
        try
        {
            List<Greeting>? database = JsonSerializer.Deserialize<List<Greeting>>(File.ReadAllText(_filename), _serializerOptions) ?? throw new Exception();
            _logger.Information("Successfully read database from file.");
            return database;
        }
        catch (Exception e)
        {
            _logger.Error($"Couldn't read database from file: {e}");
            return new List<Greeting>();
        }
    }

}
