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
using System;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Users
{
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;
        private readonly IAuthHandlerAsync _authHandler;
        private readonly IUserServiceAsync _userService;

        public CreateUser(ILogger<CreateUser> log, IUserServiceAsync userService, IAuthHandlerAsync authHandler)
        {
            _logger = log;
            _userService = userService;
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

            User user;

            try
            {
                string body = await req.ReadAsStringAsync();
                user = body.ToUser();
                await _userService.CreateUserAsync(user);

                return new OkObjectResult("User was added.");
            }
            catch  (UserAlreadyExistsException e)
            {
                return new ConflictObjectResult(e.Message);
            }
            catch (InvalidEmailException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}


