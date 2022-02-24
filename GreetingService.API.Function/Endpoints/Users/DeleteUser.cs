using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Users;

public class DeleteUser
{
    private readonly ILogger<DeleteUser> _logger;
    private readonly IUserServiceAsync _userService;
    private readonly IAuthHandlerAsync _authHandler;

    public DeleteUser(ILogger<DeleteUser> log, IUserServiceAsync userService, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _userService = userService;
        _authHandler = authHandler;
    }

    [FunctionName("DeleteGreeting")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Id is not valid guid")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "User not found")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{email}")] HttpRequest req, string email)
    {
        _logger.LogInformation("C# HTTP trigger function processed a DELETE request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

#nullable enable
        bool success = await _userService.DeleteUserAsync(email);

        return success ?
            new OkObjectResult(email + " was deleted.")
            : new NotFoundObjectResult("No such user.");
    }
}
