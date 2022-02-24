using GreetingService.API.Function.Authentication;
using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Users
{
    public class GetUser
    {
        private readonly ILogger<GetUser> _logger;
        private readonly GreetingDbContext _database;
        private readonly IAuthHandlerAsync _authHandler;

        public GetUser(ILogger<GetUser> log, GreetingDbContext database, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _database = database;
            _authHandler = authHandler;
        }

        [FunctionName("GetUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Greeting>), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Email is not valid email")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "User not found")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{email}")] HttpRequest req, string email)
        {
            _logger.LogInformation("C# HTTP trigger function processed a GET by EMAIL request.");

            if (!await _authHandler.IsAuthorizedAsync(req))
                return new UnauthorizedResult();

#nullable enable
            User? u = await _database.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (u != null)
            {
                u.Password = "";
                return new OkObjectResult(u);
            }
            else
            {
                return new StatusCodeResult(410);
            }
        }
    }
}

