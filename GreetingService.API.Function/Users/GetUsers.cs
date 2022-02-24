using GreetingService.API.Function.Authentication;
using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
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

namespace GreetingService.API.Function.Users;

public class GetUsers
{
    private readonly ILogger<GetUsers> _logger;
    private readonly IGreetingRepositoryAsync _database;
    private readonly IAuthHandlerAsync _authHandler;

    public GetUsers(ILogger<GetUsers> log, IGreetingRepositoryAsync database, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _database = database;
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

        var g = await _database.GetAsync();

        return new OkObjectResult(g);
    }
}

