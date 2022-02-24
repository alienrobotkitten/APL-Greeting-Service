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
using System.Net;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Users
{
    public class DeleteUser
    {
        private readonly ILogger<DeleteUser> _logger;
        private readonly GreetingDbContext _database;
        private readonly IAuthHandlerAsync _authHandler;

        public DeleteUser(ILogger<DeleteUser> log, GreetingDbContext database, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _database = database;
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

            User? u = await _database.Users.FirstOrDefaultAsync(user => user.Email == email);
            bool success = await _database.DeleteAsync(guid);

            return success ?
                new OkObjectResult(greetingName + " was deleted.")
                : new StatusCodeResult(410);
        }
    }
}

