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
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Greetings;

public class DeleteGreeting
{
    private readonly ILogger<DeleteGreeting> _logger;
    private readonly IGreetingRepositoryAsync _database;
    private readonly IAuthHandlerAsync _authHandler;

    public DeleteGreeting(ILogger<DeleteGreeting> log, IGreetingRepositoryAsync database, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _database = database;
        _authHandler = authHandler;
    }

    [FunctionName("DeleteGreeting")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Id is not valid guid")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Greeting not found")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "greeting/{from}/{to}/{idstring}")] HttpRequest req, string from, string to, string idstring)
    {
        _logger.LogInformation("C# HTTP trigger function processed a DELETE request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

        try
        {
            Guid id = Guid.Parse(idstring);
            bool success = await _database.DeleteAsync(id);
            
            return new OkObjectResult($"Greeting with id {id} was deleted.");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }
}
