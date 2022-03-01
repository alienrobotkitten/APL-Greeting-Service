using GreetingService.Core.Entities;

public interface IInvoiceService
{
    public Task<IEnumerable<Invoice>> GetInvoices(int year, int month);
    public Task<Invoice> GetInvoice(int year, int month, string email);
    public Task CreateOrUpdateInvoice(Invoice invoice);
}