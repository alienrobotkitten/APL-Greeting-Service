using GreetingService.Core.Entities;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GreetingService.Core.Extensions;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions _greetingSerializerOptions;
    static StringExtensions()
    {
        _greetingSerializerOptions = GreetingExtensions._serializerOptions;
    }

    public static Greeting ToGreeting(this string jsonContent)
    {
        Greeting g = JsonSerializer.Deserialize<Greeting>(jsonContent, _greetingSerializerOptions);
        return g;
    }

    public static User ToUser(this string jsonContent)
    {
        User u = JsonSerializer.Deserialize<User>(jsonContent, _greetingSerializerOptions);
        return u;
    }

    public static Invoice ToInvoice(this string jsonContent)
    {
        Invoice i = JsonSerializer.Deserialize<Invoice>(jsonContent, _greetingSerializerOptions);
        return i;
    }

    public static bool IsValidEmailAddress(this string email)
    {
        string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        Match match = Regex.Match(email, pattern);
        bool isValid = match.Success;
        //bool isValid = MailAddress.TryCreate(email, out MailAddress? result);
        return isValid;

    }
    public static string GetRandom(this string[] obj)
    {
        var random = new Random();
        int index = random.Next(0,obj.Length);
        return obj[index];
    }
}
