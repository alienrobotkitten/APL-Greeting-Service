using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Users;

public class GetUser
{
    private readonly ILogger<GetUser> _logger;
    private readonly IUserServiceAsync _userService;
    private readonly IAuthHandlerAsync _authHandler;

    public GetUser(ILogger<GetUser> log, IUserServiceAsync userService, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _userService = userService;
        _authHandler = authHandler;
    }

    [FunctionName("GetUser")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{email}")] HttpRequest req, string email)
    {
        _logger.LogInformation("C# HTTP trigger function processed a GET by EMAIL request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

        try
        {
            User user = await _userService.GetUserAsync(email);
            user.Password = "********";
            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }

    }
}


