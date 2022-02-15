using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;

using Serilog;
using System.Text.Json;

namespace GreetingService.Infrastructure;
public class MemoryGreetingRepository : IGreetingRepository
{
    private IConfiguration _config;
    private string _logfilepath;
    private List<Greeting> _greetingDatabase;
    private ILogger _logger;

    public MemoryGreetingRepository(IConfiguration config)
    {
        // _logfilepath = _config["LogFilePath"];
        _greetingDatabase = new();
        _config = config;
        _logfilepath = "./greetingrepo.log";

        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(_logfilepath)
            .CreateLogger();
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

            return true;
        }
    }




    // private methods
    private Greeting? GetGreeting(Guid id)
    {
        var greetings = from g in _greetingDatabase
                        where g.Id == id
                        select g;
        var result = greetings.FirstOrDefault();
        return result;
    }
}
