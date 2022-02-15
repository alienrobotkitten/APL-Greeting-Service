namespace GreetingService.API.Client;

public class Greeting
{
    public DateTime timestamp { get; set; } = DateTime.Now;
    public string message { get; set; } = "(empty)";
    public string from { get; set; } = "(empty)";
    public string to { get; set; } = "(empty)";
    public Guid id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Initialise greetings
    /// </summary>
    public Greeting()
    {

    }

    /// <summary>
    /// Initialise greeting with message
    /// </summary>
    /// <param name="_message"></param>
    public Greeting(string _message)
    {
        message = _message;
    }

    /// <summary>
    /// init greeting with guid and message
    /// </summary>
    /// <param name="id"></param>
    /// <param name="_message"></param>
    public Greeting(Guid id, string _message)
    {
        this.id = id;
        message = _message;
    }

    public Greeting(string _message, string _to, string _from)
    {
        message= _message;
        from = _from;   
        to = _to;
    }
}