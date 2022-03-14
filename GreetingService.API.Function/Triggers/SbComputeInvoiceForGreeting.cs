using System;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function.Triggers
{
    public class SbComputeInvoiceForGreeting
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<SbComputeInvoiceForGreeting> _logger;

        public SbComputeInvoiceForGreeting(ILogger<SbComputeInvoiceForGreeting> log, IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
            _logger = log;
        }

        [FunctionName("Function1")]
        public async Task RunAsync([ServiceBusTrigger("main", "greeting_compute_billing", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            Greeting g = mySbMsg.ToGreeting();
            await _invoiceService.CreateOrUpdateInvoiceAsync(g);
        }
    }
}
