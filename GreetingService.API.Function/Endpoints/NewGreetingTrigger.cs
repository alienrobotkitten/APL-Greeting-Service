using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints
{
    public class NewGreetingTrigger
    {
        private readonly IGreetingRepositoryAsync _database;
        private readonly ILogger<NewGreetingTrigger> _logger;

        public NewGreetingTrigger(ILogger<NewGreetingTrigger> log, IGreetingRepositoryAsync database)
        {
            _logger = log;
            _database = database;
        }

        [FunctionName("NewGreetingTrigger")]
        public async Task Run([ServiceBusTrigger("main", "greeting_create", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            Greeting g = mySbMsg.ToGreeting();
            bool success = await _database.CreateAsync(g);
        }
    }
}
