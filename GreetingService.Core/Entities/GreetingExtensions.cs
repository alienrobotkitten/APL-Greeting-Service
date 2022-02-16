using System.Text.Json;

namespace GreetingService.Core.Entities;

public static class GreetingExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions;

    static GreetingExtensions()
    {
        _serializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
    }

    /// <summary>
    /// Takes a json string and returns a Greeting. Property names are not case-sensitive.
    /// </summary>
    /// <param name="json"></param>
    /// <returns>A object of class Greeting</returns>
    public static Greeting ToGreeting(this string jsonContent)
    {
        Greeting g = JsonSerializer.Deserialize<Greeting>(jsonContent, _serializerOptions);
        return g;
    }

    /// <summary>
    /// Returns a string of this greeting serialized to json.
    /// </summary>
    /// <returns>Json string</returns>
    public static string ToJson(this Greeting greetingObject)
    {
        string s = JsonSerializer.Serialize(greetingObject, _serializerOptions);
        return s;
    }
}
