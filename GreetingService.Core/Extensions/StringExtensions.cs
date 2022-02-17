using GreetingService.Core.Entities;
using System.Text.Json;

namespace GreetingService.Core.Extensions;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions _greetingSerializerOptions;
    static StringExtensions()
    {
        _greetingSerializerOptions = GreetingExtensions._serializerOptions;
    }

    /// <summary>
    /// Takes a json string and returns a Greeting. Property names are not case-sensitive.
    /// </summary>
    /// <param name="json"></param>
    /// <returns>A object of class Greeting</returns>
    public static Greeting ToGreeting(this string jsonContent)
    {
        Greeting g = JsonSerializer.Deserialize<Greeting>(jsonContent, _greetingSerializerOptions);
        return g;
    }
}
