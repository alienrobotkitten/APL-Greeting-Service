using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace GreetingService.API.Client;

public class Program
{
    private static HttpClient _httpclient = new();

    private static bool noExit = true;
    private static string _sender = "Default sender";
    private static string _recipient = "Default recipient";

    public static JsonSerializerOptions _serializerOptions;
    private static XMLGreetingWriter _xmlGreetingWriter = new();

    public static async Task Main(string[] args)
    {
        var authParam = Convert.ToBase64String(Encoding.UTF8.GetBytes("correct:horse"));
        _httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authParam);        //Always send this header for all requests from this HttpClient
        _httpclient.BaseAddress = new Uri("http://localhost:7071/");
        //_httpclient.BaseAddress = new Uri("http://localhost:5131/");
        //_httpclient.BaseAddress = new Uri("https://helena-webapp-dev.azurewebsites.net/");  
        //_httpclient.BaseAddress = new Uri("https://helena-function-dev.azurewebsites.net/");                                              //Always use this part of the uri in all requests sent from this HttpClient

        _serializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

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

    private static async Task RepeatCallsAsync(int count)
    {
        var greetings = await GetGreetingsAsync();
        var greeting = greetings.First();

        //init a jobs list
        var jobs = new List<int>();
        for (int i = 0; i < count; i++)
        {
            jobs.Add(i);
        }

        var stopwatch = Stopwatch.StartNew();           //use stopwatch to measure elapsed time just like a real world stopwatch

        //I cheat by running multiple calls in parallel for maximum throughput - we will be limited by our cpu, wifi, internet speeds
        //This is a bit advanced and the syntax is new with lamdas - don't worry if you don't understand all of it.
        //I always copy this from the internet and adapt to my needs
        //Running this in Visual Studio debugger is slow, try running .exe file directly from File Explorer or command line prompt
        long latencySum = 0;
        await Parallel.ForEachAsync(jobs, new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (job, token) =>
        {
            var start = stopwatch.ElapsedMilliseconds;
            var response = await _httpclient.GetAsync($"api/greeting/{greeting.id}");
            var end = stopwatch.ElapsedMilliseconds;
            var diff = end - start;
            latencySum += diff;

            Console.WriteLine($"Response: {response.StatusCode} - Call: {job} - latency: {diff} ms - rate/s: {job / stopwatch.Elapsed.TotalSeconds}");
        });
        stopwatch.Stop();
        Console.WriteLine("Average latency: " + latencySum/count);
    }

    // Done
    private static async Task<List<Greeting>> GetGreetingsAsync()
    {
        var result = await _httpclient.GetAsync("api/Greeting");

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
        var result = await _httpclient.GetAsync($"api/Greeting/{id.ToString()}");

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

        var result = await _httpclient.PostAsJsonAsync<Greeting>($"api/Greeting/", g, _serializerOptions);

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

        var result = await _httpclient.PutAsJsonAsync<Greeting>($"api/Greeting/",
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
        else if (input.Contains("repeat"))
        {
            var times = int.Parse(input.Split(" ").ElementAt<string>(1));
            await RepeatCallsAsync(times);
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
    repeat
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


