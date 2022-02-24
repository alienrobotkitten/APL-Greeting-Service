using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
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
    private readonly GreetingDbContext _db;
    private readonly IAuthHandlerAsync _authHandler;

    public UpdateUser(ILogger<UpdateUser> log, GreetingDbContext database, IAuthHandlerAsync authHandler)
    {
        _logger = log;
        _db = database;
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

        string body = await req.ReadAsStringAsync();
        try
        {
            User user = body.ToUser();

            User toRemove = await _db.Users.FindAsync(user.Email);

            if (toRemove == null)
                return new NotFoundResult();

            await Task.Run(() => _db.Users.Remove(toRemove));
            await Task.Run(() => _db.Users.Add(user));

            return new OkObjectResult("User was updated.");

        }
        catch (Exception)
        {
            return new BadRequestResult();
        }
    }
}

