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
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Greetings
{
    public class GetRandom
    {
        private readonly ILogger<GetRandom> _logger;
        private readonly IGreetingRepositoryAsync _database;
        private readonly IAuthHandlerAsync _authHandler;

        public GetRandom(ILogger<GetRandom> log, IGreetingRepositoryAsync database, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _database = database;
            _authHandler = authHandler;
        }

        [FunctionName("GetRandom")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "random")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a GET request.");

            if (!await _authHandler.IsAuthorizedAsync(req))
                return new UnauthorizedResult();

            try
            {
                var random = new Random();
                string[] names = { "kajsa@ankeborg.com", "kalle@ankeborg.com", "musse@ankeborg.com", "mimmi@ankeborg.com", "joakim@ankeborg.com" };
                string[] messages = { "Hello", "Hi there!", "Good evening!", "Long time no see!", "Happy new year!", "Merry Xmas!","I love you!" };

                int year = random.Next(2020, 2022);
                int month = random.Next(1, 13);
                int day = random.Next(1, 29);
                int hour = random.Next(0, 24);
                int minute = random.Next(0, 60);
                int second = random.Next(0, 60);
                DateTime date = new DateTime(year, month, day, hour, minute, second);

                string sender = names.GetRandom();

                string receiver;
                do
                {
                    receiver = names.GetRandom();
                } while (sender == receiver);

                string message = messages.GetRandom();

                Greeting g = new Greeting(sender, receiver, message, date);

                return new OkObjectResult(g);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.ToString()) { StatusCode = 418 };
            }
        }
    }
}

