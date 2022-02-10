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
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.API.Function
{
    public class PostGreeting
    {
        private readonly ILogger<PostGreeting> _logger;
        private readonly IGreetingRepository _database;
        private readonly IAuthHandler _authHandler;

        public PostGreeting(ILogger<PostGreeting> log, IGreetingRepository database, IAuthHandler authHandler)
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

            if (!_authHandler.IsAuthorized(req))
                return new UnauthorizedResult();

            string body = await req.ReadAsStringAsync();
            try
            {
                Greeting g = JsonSerializer.Deserialize<Greeting>(body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                bool success = await Task.Run(() => _database.Create(g));

                return (success ? new OkResult() : new ConflictResult());
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}

