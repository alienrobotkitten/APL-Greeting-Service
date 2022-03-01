using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Endpoints.Invoices;

public class GetInvoice
{
    private readonly ILogger<GetInvoice> _logger;
    private readonly IAuthHandlerAsync _authHandler;
    private readonly IInvoiceService _invoiceService;

    public GetInvoice(ILogger<GetInvoice> log, IAuthHandlerAsync authHandler, IInvoiceService invoiceService)
    {
        _logger = log;
        _authHandler = authHandler;
        _invoiceService = invoiceService;
    }

    [FunctionName("GetInvoice")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Invoices" })]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "invoice/{year}/{month}/{email}")] HttpRequest req, string year, string month, string email)
    {
        _logger.LogInformation("C# HTTP trigger function processed a GET by ID request.");

        if (!await _authHandler.IsAuthorizedAsync(req))
            return new UnauthorizedResult();
        try
        {
            int y = Int32.Parse(year);
            int m = Int32.Parse(month);
            Invoice invoice = await _invoiceService.GetInvoiceAsync(y,m,email);

            return new OkObjectResult(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BadRequestObjectResult(ex.Message);
        }
    }
}

