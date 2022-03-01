using GreetingService.Core.Entities;
using GreetingService.Core.Exceptions;
using GreetingService.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.InvoiceServices;
public class SqlInvoiceService : IInvoiceService
{
    private GreetingDbContext _db;
    private ILogger<SqlInvoiceService> _log;
    private IConfiguration _config;

    public SqlInvoiceService(GreetingDbContext db, ILogger<SqlInvoiceService> log, IConfiguration config)
    {
        _db = db;
        _log = log;
        _config = config;
    }

    public async Task CreateOrUpdateInvoiceAsync(Invoice invoice)
    {

        await Task.Run(() => _db.Invoices.Update(invoice));
        _log.LogInformation($"Invoice with id {invoice.Id} added to database.");
        await _db.SaveChangesAsync();
    }

    public async Task<Invoice> GetInvoiceAsync(int year, int month, string email)
    {
        Invoice invoice = await Task.Run(() => _db.Invoices.FirstOrDefault(i => i.Year == year && i.Month == month && i.User.Email == email));
        if (invoice == null)
            throw new InvoiceNotFoundException(email, year, month);
        return invoice;
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int year, int month)
    {
        IEnumerable<Invoice> invoices = await Task.Run(() =>
                                    (from i in _db.Invoices
                                     where i.Year == year && i.Month == month
                                     select i)
                                    .ToList<Invoice>()
                                    );
        return invoices;
    }
}

