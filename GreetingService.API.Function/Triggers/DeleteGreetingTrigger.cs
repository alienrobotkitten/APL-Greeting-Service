using System;
using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Triggers;

public class DeleteGreetingTrigger
{
    private readonly IGreetingRepositoryAsync _database;
    private readonly ILogger<DeleteGreetingTrigger> _logger;
    private readonly IMessagingService _messagingService;

    public DeleteGreetingTrigger(ILogger<DeleteGreetingTrigger> log, IGreetingRepositoryAsync database, IMessagingService messagingService)
    {
        _logger = log;
        _database = database;
        _messagingService = messagingService;
    }

    [FunctionName("DeleteGreetingTrigger")]
    public async Task Run([ServiceBusTrigger("main", "greeting_delete", Connection = "ServiceBusConnectionString")] string mySbMsg)
    {
        _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

        try
        {
            bool success = Guid.TryParse(mySbMsg, out Guid id);
            Greeting? g = await _database.GetAsync(id);
            await _database.DeleteAsync(id);

            await _messagingService.SendAsync<int>(g.InvoiceId, ServiceBusSubject.GreetingDeleted.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Tried to delete greeting with id {mySbMsg} but it failed: {ex}");
        }

    }
}
