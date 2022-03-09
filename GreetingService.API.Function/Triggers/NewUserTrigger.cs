using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Triggers;

public class NewUserTrigger
{
    private readonly IUserServiceAsync _database;
    private readonly ILogger<NewUserTrigger> _logger;

    public NewUserTrigger(ILogger<NewUserTrigger> log, IUserServiceAsync database)
    {
        _logger = log;
        _database = database;
    }

    [FunctionName("NewUserTrigger")]
    public async Task Run([ServiceBusTrigger("main", "user_create", Connection = "ServiceBusConnectionString")] string mySbMsg)
    {
        _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

        User g = mySbMsg.ToUser();
        bool success = await _database.CreateUserAsync(g);
    }
}
