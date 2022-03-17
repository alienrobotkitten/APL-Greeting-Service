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
    public class SbReduceInvoiceOnDeletion
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<SbReduceInvoiceOnDeletion> _logger;

        public SbReduceInvoiceOnDeletion(ILogger<SbReduceInvoiceOnDeletion> log, IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
            _logger = log;
        }

        [FunctionName("SbReduceInvoiceOnDeletion")]
        public async Task RunAsync([ServiceBusTrigger("main", "invoice_reduce_billing", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            Greeting g = mySbMsg.ToGreeting();
            await _invoiceService.UpdateInvoiceOnGreetingDeletion(g.InvoiceId);
        }
    }
}
