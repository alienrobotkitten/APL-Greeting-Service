using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using System.Text.Json;
using Serilog;
using Serilog.Sinks.SystemConsole;
using Serilog.Core;

namespace GreetingService.Infrastructure;
public class FileGreetingRepository : IGreetingRepository
{
    private List<Greeting> _greetingDatabase;
    //private const string _filename = "./data/TestGreetings.json";
    private string _filename;
    private JsonSerializerOptions _serializerOptions;
    private Logger _logger = new LoggerConfiguration()
                      .WriteTo.Console()
                      .CreateLogger();

    public FileGreetingRepository() //ILogger logger)
    {
        _filename = "./data/NewGreetings.json";
       // _logger = logger;
        Init();
    }

    public FileGreetingRepository(string filename) //, ILogger logger)
    {
        _filename = filename;
        //_logger = logger;
        Init();
    }
    private void Init()
    {
        Console.WriteLine("Constructor starts...");

        _serializerOptions = new JsonSerializerOptions();
        _serializerOptions.AllowTrailingCommas = true;
        _serializerOptions.PropertyNameCaseInsensitive = true;
        _serializerOptions.WriteIndented = true;

        _greetingDatabase = ReadDatabaseFromFile();

        Console.WriteLine("Constructor finished.");
    }

    public IEnumerable<Greeting> Get()
    {
        Console.WriteLine("Retrieved all greetings.");

        _logger.Information("Retrieved all greetings.");
        return _greetingDatabase;
    }
    public Greeting? Get(Guid id)
    {
        Console.WriteLine($"Getting greeting with id: {id} from database...");

        var output = from g in _greetingDatabase
                     where g.Id == id
                     select g;

        try
        {
            Greeting g = output.First();
            Console.WriteLine("Retrieved one greeting from database.");
            return g;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    /// <summary>
    /// Creates greeting and returns true if it succeded.
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public bool Create(Greeting g)
    {
        if (Exists(g))
        {
            Console.WriteLine("Id already exists.");
            return false;
        }
        else
        {
            _greetingDatabase.Add(g);
            Console.WriteLine($"Added new greeting with id {g.Id} to database.");
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
        if (Exists(updatedGreeting))
        {
            var oldGreeting = (from gs in _greetingDatabase
                               where gs.Id == updatedGreeting.Id
                               select gs)
                             .First();

            _greetingDatabase.Remove(oldGreeting);
            Create(updatedGreeting);

            WriteDatabaseToFile();
            Console.WriteLine($"Updated greeting {updatedGreeting.Id}.");
            return true;
        }
        else
        {
            Console.WriteLine("No such id.");
            return false;
        }
    }



    // private methods
    private bool Exists(Greeting g)
    {
        var searchresult = from gs in _greetingDatabase
                           where gs.Id == g.Id
                           select gs;
        return searchresult.Any();
    }
    private void WriteDatabaseToFile()
    {
        JsonSerializerOptions? jsonSerializerConfig = new();


        string output = JsonSerializer.Serialize(_greetingDatabase, _serializerOptions);

        File.WriteAllText(_filename, output);

        Console.WriteLine("Wrote database to file.");
    }
    private List<Greeting> ReadDatabaseFromFile()
    {
        Console.WriteLine("Reading database from file...");
        try
        {
            List<Greeting>? database = JsonSerializer.Deserialize<List<Greeting>>(File.ReadAllText(_filename), _serializerOptions) ?? throw new Exception();
            Console.WriteLine("Successfully read database from file.");
            return database;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't read database from file: {e}");
            return new List<Greeting>();

        }
    }
}
