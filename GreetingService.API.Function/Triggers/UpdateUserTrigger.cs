using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Triggers;

public class UpdateUserTrigger
{
    private readonly IUserServiceAsync _database;
    private readonly ILogger<UpdateUserTrigger> _logger;

    public UpdateUserTrigger(ILogger<UpdateUserTrigger> log, IUserServiceAsync database)
    {
        _logger = log;
        _database = database;
    }

    [FunctionName("UpdateUserTrigger")]
    public async Task Run([ServiceBusTrigger("main", "user_update", Connection = "ServiceBusConnectionString")] string mySbMsg)
    {
        _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

        User g = mySbMsg.ToUser();
        bool success = await _database.UpdateUserAsync(g);
    }
}
