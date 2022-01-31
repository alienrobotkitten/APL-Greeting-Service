using System.Text.Json;
using System.Net.Http.Json;

namespace GreetingService.API.Client;

public class Program
{
    private static HttpClient _httpclient = new();
    private static bool noExit = true;
    private static string _sender = "Default sender";
    private static string _recipient = "Default recipient";

    private static JsonSerializerOptions _serializerOptions;
    private static XMLGreetingWriter _xmlGreetingWriter = new();

    public static async Task Main(string[] args)
    {
        _serializerOptions = new JsonSerializerOptions();
        _serializerOptions.AllowTrailingCommas = true;
        _serializerOptions.PropertyNameCaseInsensitive = true;
        _serializerOptions.WriteIndented = true;

        Display(
@"
 __   __   ___  ___ ___         __  
/ _` |__) |__  |__   |  | |\ | / _` 
\__> |  \ |___ |___  |  | | \| \__> 
                                    
 __   ___  __          __   ___     
/__` |__  |__) \  / | /  ` |__      
.__/ |___ |  \  \/  | \__, |___     
                                    
Welcome to command line Greeting client
");

        Console.Write("Enter name of greeting sender:\n> ");
        _sender = Console.ReadLine() ?? "John";

        Console.Write("Enter name of greeting recipient:\n> ");
        _recipient = Console.ReadLine() ?? "Ellen";

        while (noExit)
        {
            DisplayMenu();
            await GetCommand();
        }
    }

    // Done
    private static async Task<List<Greeting>> GetGreetingsAsync()
    {
        var result = await _httpclient.GetAsync("http://localhost:5131/api/Greeting");

        if (result.IsSuccessStatusCode)
        {
            var contentBody = await result.Content.ReadAsStringAsync();
            var parsedJson = ParseJson(contentBody);
            return parsedJson;
        }
        else
            return new();
    }



    // Done
    private static async Task<Greeting> GetGreetingAsync(Guid id)
    {
        var result = await _httpclient.GetAsync($"http://localhost:5131/api/Greeting/{id.ToString()}");

        if (result.IsSuccessStatusCode)
        {
            var contentBody = await result.Content.ReadAsStringAsync();
            try
            {
                Greeting? greeting = JsonSerializer.Deserialize<Greeting>(contentBody, _serializerOptions)
                    ?? throw new FormatException();
                Display(greeting);
                return greeting;
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't parse json.");
                return new();
            }
        }
        else
            return new();
    }

    // Done
    private static async Task WriteGreetingAsync(string message)
    {
        Greeting g = new(
            _from: _sender,
            _to: _recipient,
            _message: message);

        var result = await _httpclient.PostAsJsonAsync<Greeting>($"http://localhost:5131/api/Greeting/", g, _serializerOptions);

        if (result.IsSuccessStatusCode)
        {
            Display("Wrote greeting. Service responded with: OK");
        }
        else
        {
            Display("Tried to write greeting. Service responded with: Request failed.");
        }
    }

    // Done
    private static async Task<string> UpdateGreetingAsync(Guid id, string message)
    {
        Greeting upDatedGreeting = await GetGreetingAsync(id);

        upDatedGreeting.message = message;

        var result = await _httpclient.PutAsJsonAsync<Greeting>($"http://localhost:5131/api/Greeting/{id.ToString()}",
            upDatedGreeting,
            _serializerOptions);

        if (result.IsSuccessStatusCode)
        {
            Display("Updated greeting. Service responded with: OK");
        }
        else
        {
            Display("Tried to write greeting. Service responded with: Request failed.");
        }

        return "OK";
    }

    private static async Task ExportGreetingsAsync()
    {
        List<Greeting> greetings = await GetGreetingsAsync();
        _xmlGreetingWriter.Write(greetings);
        Display("Exported greetings to file as XML.");
    }


    /// <summary>
    /// Parses command written in command line interface and calls correct method.
    /// </summary>
    /// <returns>void</returns>
    private static async Task GetCommand()
    {
        string? input = Console.ReadLine();
        if (input == null)
            return;
        input = input.Trim();

        if (input == "get greetings")
        {
            List<Greeting> greetings = await GetGreetingsAsync();
        }
        else if (input.Contains("get greeting["))
        {
            string[] splitInput = GetValueFromInput(input);

            try
            {
                Guid id = Guid.Parse(splitInput[1]);
                Greeting greeting = await GetGreetingAsync(id);

            }
            catch (Exception)
            {
                Display("Invalid id.");
            }
        }
        else if (input.Contains("write greeting["))
        {
            string[] splitInput = GetValueFromInput(input);
            string message = splitInput[1];
            await WriteGreetingAsync(message);
        }
        else if (input.Contains("export"))
        {
            await ExportGreetingsAsync();

        }
        else if (input.Contains("update greeting["))
        {
            string[] splitInput = GetValueFromInput(input);
            try
            {
                Guid id = Guid.Parse(splitInput[1]);
                string message = splitInput[2].Length > 2 ? splitInput[2] : splitInput[3];
                await UpdateGreetingAsync(id, message);
            }
            catch (Exception)
            {
                Display("Invalid id");
            }
        }
        else if (input.Contains("exit"))
        {
            noExit = false;
        }
        else
        {
            Console.WriteLine("Invalid command.");
            return;
        }
    }
    // private methods
    /// <summary>
    /// Displays a menu to the user.
    /// </summary>
    private static void DisplayMenu()
    {
        Console.WriteLine(
          @"
Available commands:
            
    get greetings
    get greeting[id]
    write greeting[message]
    update greeting[id][message]
    export
    exit

Write command and press [enter] to execute");
        Console.Write("> ");
    }


    /// <summary>
    /// Parses values from input, splitting between [ or ]
    /// </summary>
    /// <param name="input">string to parse</param>
    /// <returns>string[] with values</returns>
    private static string[] GetValueFromInput(string input)
    {
        char[] delimiterChars = { '[', ']' };
        return input.Split(delimiterChars);
    }

    private static List<Greeting> ParseJson(string contentBody)
    {
        try
        {
            List<Greeting>? greetings = JsonSerializer.Deserialize<List<Greeting>>(contentBody, _serializerOptions)
                ?? throw new FormatException();
            Display(greetings);
            return greetings;
        }
        catch (Exception)
        {
            Console.WriteLine("Couldn't parse json.");
            return new();
        }
    }

    private static void Display(List<Greeting> greetings)
    {
        foreach (var g in greetings)
        {
            Display(g);
        }
    }

    private static void Display(Greeting g)
    {
        Console.WriteLine($"[{g.id}] [{g.timestamp}] ({g.from}->{g.to}) - {g.message}");
    }

    private static void Display(string confirmation)
    {
        Console.WriteLine(confirmation);
    }


}


