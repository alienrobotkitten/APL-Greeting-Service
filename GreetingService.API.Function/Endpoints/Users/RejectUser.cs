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
    public class RejectUser
    {
        private readonly ILogger<RejectUser> _logger;
        private readonly IUserServiceAsync _userService;

        public RejectUser(ILogger<RejectUser> log, IUserServiceAsync userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("RejectUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Users" })]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/reject/{approvalCode}")] HttpRequest req, Guid approvalCode)
        {
            _logger.LogInformation("C# HTTP trigger function processed a rejection request.");

            try
            {
                await _userService.RejectUserAsync(approvalCode);

                return new OkObjectResult("User was rejected.");
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }
    }
}


