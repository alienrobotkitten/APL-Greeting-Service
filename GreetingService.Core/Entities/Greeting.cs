
﻿using System.Text.Json;

namespace GreetingService.Core.Entities;

public class Greeting
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Message { get; set; } = "(empty)";
    public string From { get; set; } = "(empty)";
    public string To { get; set; } = "(empty)";
    public Guid Id { get; set; } = Guid.NewGuid();

    public Greeting()
    {
        Id = Guid.NewGuid();
        Message = "(empty)";
        From = "(empty)";
        To = "(empty)";
        Timestamp = DateTime.Now;
    }

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
    public Greeting(string from, string to, string message, Guid id, DateTime timestamp)
    {
        Timestamp = timestamp;
        From = from;
        To = to;
        Message = message;
        Id = id;
    }
}


