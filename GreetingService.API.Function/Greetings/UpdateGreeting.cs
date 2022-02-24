using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Greetings;

public class UpdateGreeting
{
    private readonly ILogger<UpdateGreeting> _logger;
    private readonly IGreetingRepositoryAsync _database;
    private readonly IAuthHandlerAsync _authHandler;

    public UpdateGreeting(ILogger<UpdateGreeting> log, IGreetingRepositoryAsync database, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _database = database;
        _authHandler = authHandler;
    }

    [FunctionName("PutGreeting")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Greeting didn't exist")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "greeting")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a PUT request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

        string body = await req.ReadAsStringAsync();
        try
        {
            Greeting g = body.ToGreeting();
            bool success = await _database.UpdateAsync(g);

            return success ?
                new OkObjectResult("Greeting was updated.")
                : new StatusCodeResult(410);
        }
        catch (Exception)
        {
            return new BadRequestResult();
        }
    }
}

