﻿using GreetingService.API.Function.Authentication;
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

namespace GreetingService.API.Function
{
    public class CoffeeGreeting
    {
        private readonly ILogger<CoffeeGreeting> _logger;
        private readonly IGreetingRepositoryAsync _database;
        private readonly IAuthHandler _authHandler;

        public CoffeeGreeting(ILogger<CoffeeGreeting> log, IGreetingRepositoryAsync database, IAuthHandler authHandler)
        {
            _logger = log;
            _database = database;
            _authHandler = authHandler;
        }

        [FunctionName("Coffee")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Time for a break?" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "coffee")] HttpRequest req)
        {
            _logger.LogInformation("Client asked for coffee.");

            return new ObjectResult("418 I'm a teapot.") { StatusCode = 418 };
           // return new StatusCodeResult(418);  // I'm a teapot.
        }
    }
}

