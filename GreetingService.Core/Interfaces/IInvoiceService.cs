using GreetingService.Core.Entities;

namespace GreetingService.Core.Interfaces;

public interface IInvoiceService
{
    public Task<IEnumerable<Invoice>> GetInvoicesAsync(int year, int month);
    public Task<Invoice> GetInvoiceAsync(int year, int month, string email);
    public Task CreateOrUpdateInvoiceAsync(Invoice invoice);
    public Task CreateOrUpdateInvoiceAsync(Greeting greeting);
    public Task ProcessGreetingsForInvoices();

    public Task UpdateInvoiceOnGreetingDeletion(int invoiceId);
}