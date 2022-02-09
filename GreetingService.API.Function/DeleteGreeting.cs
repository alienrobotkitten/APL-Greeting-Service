using GreetingService.API.Function.Authentication;
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
using System.Net;
using System.Threading.Tasks;

namespace GreetingService.API.Function
{
    public class DeleteGreeting
    {
        private readonly ILogger<DeleteGreeting> _logger;
        private readonly IGreetingRepository _database;
        private readonly IAuthHandler _authHandler;

        public DeleteGreeting(ILogger<DeleteGreeting> log, IGreetingRepository database, IAuthHandler authHandler)
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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "greeting/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a DELETE request.");

            if (!_authHandler.IsAuthorized(req))
                return new UnauthorizedResult();

            if (!Guid.TryParse(id, out var guid))
                return new BadRequestObjectResult($"{id} is not a valid Guid");

            bool success = await Task.Run(() => _database.Delete(guid));

            return success ? new OkResult() : new NotFoundResult();
        }
    }
}

