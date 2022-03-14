using GreetingService.Core.Exceptions;
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
    public class ApproveUser
    {
        private readonly ILogger<ApproveUser> _logger;
        private readonly IUserServiceAsync _userService;

        public ApproveUser(ILogger<ApproveUser> log, IUserServiceAsync userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("ApproveUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/approve/{approvalCode}")] HttpRequest req, Guid approvalCode)
        {
            _logger.LogInformation("C# HTTP trigger function processed an approval request.");

            try
            {
                await _userService.ApproveUserAsync(approvalCode);

                return new OkObjectResult("User was approved.");
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }
    }
}


