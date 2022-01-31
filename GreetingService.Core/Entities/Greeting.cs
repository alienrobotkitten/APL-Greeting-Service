namespace GreetingService.Core.Entities;

public class Greeting
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public Guid Id { get; set; }

    public Greeting(DateTime timestamp, string from, string to, string message, Guid id)
    {
        Timestamp = timestamp;
        From = from;
        To = to;
        Message = message;
        Id = id;
    }
}


