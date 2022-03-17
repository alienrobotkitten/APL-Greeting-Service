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
using GreetingService.Core.Entities;

namespace GreetingService.API.Function.Endpoints.Greetings;

public class DeleteGreeting
{
    private readonly ILogger<DeleteGreeting> _logger;
    private readonly IMessagingService _messagingService;
    private readonly IAuthHandlerAsync _authHandler;

    public DeleteGreeting(ILogger<DeleteGreeting> log, IMessagingService messagingService, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _messagingService = messagingService;
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
            await _messagingService.SendAsync<string>(idstring, ServiceBusSubject.DeleteGreeting.ToString());

            return new OkObjectResult($"Greeting with id {idstring} was queued for deletion.");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }
}
