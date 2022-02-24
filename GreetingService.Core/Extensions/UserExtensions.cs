using GreetingService.Core.Entities;
using System.Text.Json;

namespace GreetingService.Core.Extensions;

public static class UserExtensions
{
    public static readonly JsonSerializerOptions _serializerOptions;

    static UserExtensions()
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
    public static string ToJson(this User u)
    {
        string s = JsonSerializer.Serialize<User>(u, _serializerOptions);
        return s;
    }
}
