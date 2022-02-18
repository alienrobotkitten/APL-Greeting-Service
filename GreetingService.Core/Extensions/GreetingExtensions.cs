using GreetingService.Core.Entities;
using System.Text.Json;

namespace GreetingService.Core.Extensions;

public static class GreetingExtensions
{
    public static readonly JsonSerializerOptions _serializerOptions;

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
    /// Returns a string of this greeting serialized to json.
    /// </summary>
    /// <returns>Json string</returns>
    public static string ToJson(this Greeting greetingObject)
    {
        string s = JsonSerializer.Serialize<Greeting>(greetingObject, _serializerOptions);
        return s;
    }

    public static Stream ToCsvStream(this Greeting greetingObject, string delimiter = ";")
    {
        MemoryStream outputStream = new();
        StreamWriter streamWriter = new(outputStream);

        string csvString = ToCsvString(greetingObject, delimiter);

        streamWriter.Write(csvString);
        streamWriter.Flush();

        return outputStream;
    }

    public static string ToCsvString(this Greeting greetingObject, string delimiter = ";")
    {
        string stringOutput = $"{greetingObject.Id}{delimiter}" +
            $"{greetingObject.From}{delimiter}" +
            $"{greetingObject.To}{delimiter}" +
            $"{greetingObject.Message}{delimiter}" +
            $"{greetingObject.Timestamp}";
        return stringOutput;
    }
}
