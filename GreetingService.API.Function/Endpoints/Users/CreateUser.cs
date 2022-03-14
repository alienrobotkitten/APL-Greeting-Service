using GreetingService.Core.Entities;
using GreetingService.Core.Exceptions;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using System;

namespace GreetingService.API.Function.Endpoints.Users
{
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;
        private readonly IAuthHandlerAsync _authHandler;
        private readonly IMessagingService _messagingService;

        public CreateUser(ILogger<CreateUser> log, IMessagingService messagingService, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _messagingService = messagingService;
            _authHandler = authHandler;
        }

        [FunctionName("CreateUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a POST request.");

            //if (!await _authHandler.IsAuthorizedAsync(req))
            //    return new UnauthorizedResult();

            try
            {
                string body = await req.ReadAsStringAsync();
                User user = body.ToUser();

                user.ApprovalCode = Guid.NewGuid();
                user.ApprovalStatus = UserStatus.Pending;
                user.ApprovalStatusNote = "Waiting for approval by admin.";
                user.ApprovalExpiry = DateTime.Now.AddDays(1);

                await _messagingService.SendAsync<User>(user, ServiceBusSubject.NewUser.ToString());

                return new OkObjectResult("User was added.");
            }
            catch (InvalidEmailException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}


