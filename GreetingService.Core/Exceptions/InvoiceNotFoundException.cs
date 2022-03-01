using GreetingService.Core.Entities;
using System.Runtime.Serialization;

[Serializable]
public class InvoiceNotFoundException : Exception
{
    public string? Email { get; private set; }
    public int? Year { get; private set; }
    public int? Month { get; private set; }
    private string? _message;
    public override string Message
    {
        get
        {
            return _message;
        }
    }

    //public InvoiceNotFoundException()
    //{
    //}

    public InvoiceNotFoundException(string email, int year, int month)
    {
       Email = email;
        Year = year;
        Month = month;
        _message = $"Invoice for user {Email} from {Year}-{Month} could not be found.";
    }

    //public InvoiceNotFoundException(string? message) : base(message)
    //{

    //}

    //public InvoiceNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    //{
    //}

    public InvoiceNotFoundException(string email, int year, int month, Exception innerException)
    {
        Email = email;
        Year = year;
        Month = month;
        _message = $"Invoice for user {email} from {year}-{month} could not be found.";
    }

    protected InvoiceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}