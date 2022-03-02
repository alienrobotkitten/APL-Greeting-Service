using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GreetingService.API.Function
{
    public class GenerateInvoices
    {
        [FunctionName("GenerateInvoices")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] ILogger log, IInvoiceService invoiceService)
        {



        }
    }
}
