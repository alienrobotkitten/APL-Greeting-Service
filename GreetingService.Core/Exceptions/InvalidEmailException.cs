using System;
using System.Runtime.Serialization;

namespace GreetingService.Core.Exceptions;

[Serializable]
public class InvalidEmailException : Exception
{
    public string Email { get; private set; }
    private string _message;
    public override string Message
    {
        get
        {
            return _message;
        }
    }
    public InvalidEmailException()
    {
    }

    public InvalidEmailException(string message) : base(message)
    {
        Email = message;
        _message = $"{Email} is not a valid e-mail address.";
    }

    public InvalidEmailException(string message, Exception innerException) : base(message, innerException)
    {
        Email = message;
        _message = $"{Email} is not a valid e-mail address.";
    }

    protected InvalidEmailException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}