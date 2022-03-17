using System.ComponentModel.DataAnnotations;
using GreetingService.Core.Extensions;
using GreetingService.Core.Exceptions;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace GreetingService.Core.Entities;

public class Greeting
{
    public int InvoiceId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Message { get; set; }
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    private string _to;
    public string To
    {
        get => _to;
        set
        {
            bool isValid = value.IsValidEmailAddress();
            if (!isValid)
                throw new InvalidEmailException(value);
            _to = value;
        }
    }

    private string _from;
    public string From
    {
        get => _from;
        set
        {
            bool isValid = value.IsValidEmailAddress();
            if (!isValid)
                throw new InvalidEmailException(value);
            _from = value;
        }
    }

    public Greeting()
    {

    }

    /// <summary>
    /// Makes a greeting with current date and time and a new guid
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="message"></param>
    [JsonConstructorAttribute]
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

    public Greeting(string from, string to, string message, Guid id, DateTime timestamp)
    {
        Timestamp = timestamp;
        From = from;
        To = to;
        Message = message;
        Id = id;
    }
}


