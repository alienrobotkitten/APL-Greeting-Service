using System.Runtime.Serialization;

namespace GreetingService.Core.Exceptions;

[Serializable]
public class UserDoesNotExistException : Exception
{
    public string UserName { get; private set; }
    private string _message;
    public override string Message
    {
        get
        {
            return _message;
        }
    }
    public UserDoesNotExistException()
    {
    }

    public UserDoesNotExistException(string message) : base(message)
    {
        UserName = message;
        _message = $"There is no user with username {UserName}";
    }

    public UserDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
        UserName = message;
        _message = $"There is no user with username {UserName}";
    }

    protected UserDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
