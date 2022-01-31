using System;
using GreetingService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
/// <summary>
/// Summary description for Class1
/// </summary>


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthAttribute : TypeFilterAttribute
{
    public BasicAuthAttribute(string realm = @"My Realm") : base(typeof(BasicAuthFilter))
    {
        Arguments = new object[] { realm };
    }

    [HttpGet("basic")]
    [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
    public IEnumerable<int> BasicAuth()
    {
        //_logger.LogInformation("basic auth");
        var rng = new Random();
        return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
    }
}
