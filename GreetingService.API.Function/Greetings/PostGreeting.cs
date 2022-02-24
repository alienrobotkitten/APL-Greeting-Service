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
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Greetings
{
    public class PostGreeting
    {
        private readonly ILogger<PostGreeting> _logger;
        private readonly IGreetingRepositoryAsync _database;
        private readonly IAuthHandlerAsync _authHandler;

        public PostGreeting(ILogger<PostGreeting> log, IGreetingRepositoryAsync database, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _database = database;
            _authHandler = authHandler;
        }

        [FunctionName("PostGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict, Description = "Greeting already exists")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "greeting")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a POST request.");

            if (!await _authHandler.IsAuthorizedAsync(req))
                return new UnauthorizedResult();

            string body = await req.ReadAsStringAsync();
            try
            {
                Greeting g = body.ToGreeting();
                bool success = await _database.CreateAsync(g);

                return success ?
                    new OkObjectResult("Greeting was created.")
                    : new ConflictObjectResult($"Greeting with guid {g.Id} already exists.");
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}

