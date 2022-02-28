using System.Runtime.Serialization;

namespace GreetingService.Core.Exceptions;

[Serializable]
public class GreetingNotFoundException : Exception
{
    public string Id { get; private set; }

    private string _message;
    public override string Message
    {
        get 
        { 
            return _message; 
        }
    }
    public GreetingNotFoundException()
    {
    }

    public GreetingNotFoundException(string message) : base(message)
    {
        Id = message;
        _message = $"Greeting with {Id} does not exist.";
    }

    public GreetingNotFoundException(string message, Exception? innerException) : base(message, innerException)
    {
        Id = message;
        _message = $"Greeting with {Id} does not exist.";
    }

    protected GreetingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

}