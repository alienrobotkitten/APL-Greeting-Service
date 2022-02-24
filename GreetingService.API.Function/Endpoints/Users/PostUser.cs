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
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Users
{
    public class PostUser
    {
        private readonly ILogger<PostUser> _logger;
        private readonly GreetingDbContext _db;
        private readonly IAuthHandlerAsync _authHandler;

        public PostUser(ILogger<PostUser> log, GreetingDbContext database, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _db = database;
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
                User user = body.ToUser();

                User existingUser = await _db.Users.FindAsync(user.Email);

                if (existingUser != null)
                    return new ConflictResult();

                await Task.Run(() => _db.Users.Add(user));

                return new OkObjectResult("User was added.");

            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}

