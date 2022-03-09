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

namespace GreetingService.API.Function.Endpoints.Greetings
{
    public class PostGreeting
    {
        private readonly ILogger<PostGreeting> _logger;
        private readonly IAuthHandlerAsync _authHandler;
        private readonly IMessagingService _messagingService;

        public PostGreeting(ILogger<PostGreeting> log, IAuthHandlerAsync authHandler, IMessagingService messagingService)
        {
            _logger = log;
            _authHandler = authHandler;
            _messagingService = messagingService;
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

                await _messagingService.SendAsync<Greeting>(g, ServiceBusSubject.NewGreeting.ToString());

                return new OkObjectResult("Greeting was queued.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}

