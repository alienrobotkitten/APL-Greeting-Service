using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GreetingService.Core.Entities;

public class Greeting
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    // private static readonly JsonSerializerSettings _serializerOptions;

    /// <summary>
    /// Makes a greeting with current date and time and a new guid
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="message"></param>
    public Greeting(string from, string to, string message)
    {
        From = from;
        To = to;
        Message = message;
    }

    /// <summary>
    /// Makes a greeting with specified guid and current date and time
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="message"></param>
    /// <param name="id"></param>
    public Greeting(string from, string to, string message, Guid id)
    {
        From = from;
        To = to;
        Message = message;
        Id = id;
    }

    /// <summary>
    /// Makes a greeting with a new guid.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="message"></param>
    /// <param name="timestamp"></param>
    public Greeting(string from, string to, string message, DateTime timestamp)
    {
        From = from;
        To = to;
        Message = message;
        Timestamp = timestamp;
    }

    /// <summary>
    /// Makes a greeting with all properties specified.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="message"></param>
    /// <param name="id"></param>
    /// <param name="timestamp"></param>
    [JsonConstructor]
    public Greeting(string from, string to, string message, Guid id, DateTime timestamp)
    {
        Timestamp = timestamp;
        From = from;
        To = to;
        Message = message;
        Id = id;
    }

    /// <summary>
    /// Initializes serializer options for the json serializer and deserializer.
    /// </summary>
    static Greeting()
    {
        //_serializerOptions = new()
        //{
        //    AllowTrailingCommas = true,
        //    PropertyNameCaseInsensitive = true,
        //    WriteIndented = true
        //};
    }

    /// <summary>
    /// Takes a json string and returns a Greeting. Property names are not case-sensitive.
    /// </summary>
    /// <param name="json"></param>
    /// <returns>A object of class Greeting</returns>
    public static Greeting JsonDeserialize(string json)
    {
        Greeting g = JsonConvert.DeserializeObject<Greeting>(json/*,_serializerOptions*/);
        return g;
    }

    /// <summary>
    /// Returns a string of this greeting serialized to json.
    /// </summary>
    /// <returns>Json string</returns>
    public string ToJson()
    {
        string s = JsonConvert.SerializeObject(this/*, _serializerOptions*/);
        return s;
    }
}
