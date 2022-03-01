using GreetingService.Core.Entities;
using System.Text.Json;

namespace GreetingService.Core.Extensions;

public static class InvoiceExtensions
{
    public static readonly JsonSerializerOptions _serializerOptions;

    static InvoiceExtensions()
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
    public static string ToJson(this Invoice invoice)
    {
        string jsonstring= JsonSerializer.Serialize<Invoice>(invoice, _serializerOptions);
        return jsonstring;
    }
}
