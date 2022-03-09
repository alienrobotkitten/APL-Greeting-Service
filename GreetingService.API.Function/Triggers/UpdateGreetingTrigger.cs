using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Triggers;

public class UpdateGreetingTrigger
{
    private readonly IGreetingRepositoryAsync _database;
    private readonly ILogger<UpdateGreetingTrigger> _logger;

    public UpdateGreetingTrigger(ILogger<UpdateGreetingTrigger> log, IGreetingRepositoryAsync database)
    {
        _logger = log;
        _database = database;
    }

    [FunctionName("UpdateGreetingTrigger")]
    public async Task Run([ServiceBusTrigger("main", "greeting_update", Connection = "ServiceBusConnectionString")] string mySbMsg)
    {
        _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

        Greeting g = mySbMsg.ToGreeting();
        bool success = await _database.UpdateAsync(g);
    }
}
