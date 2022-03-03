using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GreetingService.API.Function
{
    public class InvoicesTrigger
    {
        private IInvoiceService _invoiceService;
        private ILogger _log;

        public InvoicesTrigger(ILogger log, IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
            _log = log;
        }

        [FunctionName("InvoicesTrigger")]
        public async Task Run([TimerTrigger("*/5 * * * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            try
            {
                await _invoiceService.ProcessGreetingsForInvoices();
            }
            catch (Exception ex)
            {
                _log.LogError("Greeting processing for invoices failed", ex);
            }
            _log.LogInformation("Greetings were processed and invoices generated.");
        }
    }
}
