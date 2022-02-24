using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Users;

public class GetUsers
{
    private readonly ILogger<GetUsers> _logger;
    private readonly GreetingDbContext _db;
    private readonly IAuthHandlerAsync _authHandler;

    public GetUsers(ILogger<GetUsers> log, GreetingDbContext database, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _db = database;
        _authHandler = authHandler;
    }

    [FunctionName("GetGreetings")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "greeting")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a GET request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

        var usersQueryable = _db.Users.AsQueryable();
        var users = new List<User>();
        foreach (var user in usersQueryable)
            users.Add(user);

        return new OkObjectResult(users);
    }
}

