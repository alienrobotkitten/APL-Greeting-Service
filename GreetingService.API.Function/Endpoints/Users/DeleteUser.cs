using GreetingService.Core.Exceptions;
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

    [FunctionName("DeleteUser")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{email}")] HttpRequest req, string email)
    {
        _logger.LogInformation("C# HTTP trigger function processed a DELETE request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();

#nullable enable
        try
        {
            await _userService.DeleteUserAsync(email);

            return new OkObjectResult(email + " was deleted.");
        }
        catch (InvalidEmailException ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
        catch (UserDoesNotExistException ex)
        {
            return new NotFoundObjectResult(ex.Message);
        }
    }
}
