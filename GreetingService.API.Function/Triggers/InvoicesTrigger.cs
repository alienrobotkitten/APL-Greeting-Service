using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Triggers;

public class InvoicesTrigger
{
    private const string cronSchedule = "0 0 */12 * * *";
    private IInvoiceService _invoiceService;
    private ILogger<InvoicesTrigger> _log;

    public InvoicesTrigger(ILogger<InvoicesTrigger> log, IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        _log = log;
    }

    [FunctionName("InvoicesTrigger")]
    public async Task Run([TimerTrigger(cronSchedule, RunOnStartup = true)] TimerInfo myTimer)
    {
        try
        {
            await _invoiceService.ProcessGreetingsForInvoices();
            _log.LogInformation("Greetings were processed and invoices generated.");
        }
        catch (Exception ex)
        {
            _log.LogError("Greeting processing for invoices failed", ex);
        }

    }
}
