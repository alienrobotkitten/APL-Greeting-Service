using System.Runtime.Serialization;

namespace GreetingService.Core.Exceptions;

[Serializable]
public class UserAlreadyExistsException : Exception
{
    public string UserName { get; private set; }
    private string _message;
    public override string Message { 
        get {
            return _message; 
        }
    }
    public UserAlreadyExistsException()
    {
    }

    public UserAlreadyExistsException(string message) : base(message)
    {
        UserName = message;
        _message = $"A user with username {UserName} already exists.";
    }

    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
        UserName = message;
        _message = $"A user with username {UserName} already exists.";
    }

    protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}