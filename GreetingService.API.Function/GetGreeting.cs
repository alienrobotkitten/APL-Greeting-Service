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
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GreetingService.API.Function
{
    public class GetGreeting
    {
        private readonly ILogger<GetGreeting> _logger;
        private readonly IGreetingRepositoryAsync _database;
        private readonly IAuthHandler _authHandler;

        public GetGreeting(ILogger<GetGreeting> log, IGreetingRepositoryAsync database, IAuthHandler authHandler)
        {
            _logger = log;
            _database = database;
            _authHandler = authHandler;
        }

        [FunctionName("GetGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Greeting>), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Id is not valid guid")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Greeting not found")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "greeting/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a GET by ID request.");

            if (!_authHandler.IsAuthorized(req))
                return new UnauthorizedResult();

            if (!Guid.TryParse(id, out var guid))
                return new BadRequestObjectResult($"{id} is not a valid Guid");

            #nullable enable
            Greeting? g = await _database.GetAsync(guid);

            return g != null ? 
                new OkObjectResult(g) 
                : new StatusCodeResult(410);
        }
    }
}

