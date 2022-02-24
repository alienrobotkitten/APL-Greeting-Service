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
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Users;

public class UpdateUser
{
    private readonly ILogger<UpdateUser> _logger;
    private readonly IUserServiceAsync _userService;
    private readonly IAuthHandlerAsync _authHandler;

    public UpdateUser(ILogger<UpdateUser> log, IUserServiceAsync userService, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _userService = userService;
        _authHandler = authHandler;
    }

    [FunctionName("PutGreeting")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "User didn't exist")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "greeting")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a PUT request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

        User user;

        try
        {
            string body = await req.ReadAsStringAsync();
            user = body.ToUser();
        }
        catch (Exception ex)
        {
            return new UnprocessableEntityObjectResult(ex);
        }

        bool success = await _userService.UpdateUserAsync(user);

        return success ?
            new OkObjectResult("User was added.")
            : new NotFoundObjectResult("No such user.");
    }
}

