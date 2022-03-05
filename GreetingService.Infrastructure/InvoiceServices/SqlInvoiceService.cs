using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure.InvoiceServices;

public class SqlInvoiceService : IInvoiceService
{
    private GreetingDbContext _dataBase;
    private ILogger<SqlInvoiceService> _log;
    private IConfiguration _config;
    private IGreetingRepositoryAsync _greetingRepository;
    private IUserServiceAsync _userService;

    public SqlInvoiceService(GreetingDbContext db,
        ILogger<SqlInvoiceService> log,
        IConfiguration config,
        IGreetingRepositoryAsync grepo,
        IUserServiceAsync us)
    {
        _dataBase = db;
        _log = log;
        _config = config;
        _greetingRepository = grepo;
        _userService = us;

        var canConnect = _dataBase.Database.CanConnect();
        if (!canConnect)
            _log.LogError("Can't connect to database.");
    }

    public async Task CreateOrUpdateInvoiceAsync(Invoice invoice)
    {
        await Task.Run(() => _dataBase.Invoices.Update(invoice));
        _log.LogInformation($"Invoice with id {invoice.Id} added to database or updated.");
        await _dataBase.SaveChangesAsync();
    }

    public async Task<Invoice> GetInvoiceAsync(int year, int month, string email)
    {
        Invoice invoice = await Task.Run(() => _dataBase.Invoices.FirstOrDefault(i => i.Year == year && i.Month == month && i.User.Email == email));
        if (invoice == null)
            throw new InvoiceNotFoundException(email, year, month);
        return invoice;
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int year, int month)
    {
        IEnumerable<Invoice> invoices = await Task.Run(() =>
                                    (from i in _dataBase.Invoices
                                     where i.Year == year && i.Month == month
                                     select i)
                                    .ToList<Invoice>()
                                    );
        return invoices;
    }
    public async Task ProcessGreetingsForInvoices()
    {
        var greetings = from g in _dataBase.Greetings
                        where g.InvoiceId == null
                        select g;

        //foreach (var greeting in greetings)
        //{
        //    greeting.InvoiceId = null;
        //    _dataBase.Greetings.Update(greeting);
        //}

        //await _dataBase.SaveChangesAsync();

        List<Greeting> greetingsList = greetings.ToList();

        var users = from u in _dataBase.Users
                    select u;
        List<User> usersList = users.ToList();

        var invoices = from i in _dataBase.Invoices
                       select i;

        foreach (var greeting in greetingsList)
        {
            string userEmail = greeting.From;
            int month = greeting.Timestamp.Month;
            int year = greeting.Timestamp.Year;

            Invoice? invoice = invoices.FirstOrDefault(i => i.User.Email == userEmail
                                                            && i.Month == month
                                                            && i.Year == year);
            if (invoice == null)
            {
                invoice = new Invoice();
                invoice.User = usersList.FirstOrDefault(u => u.Email == userEmail);
                invoice.Year = year;
                invoice.Month = month;
                invoice.Cost = 13.37F;
                invoice.TotalCost = 0;
                invoice.Currency = "SEK";
            }

            invoice.TotalCost += invoice.Cost;
            await CreateOrUpdateInvoiceAsync(invoice);

            if (invoice.Id == null)
            {
                var invoiceId = (from i in _dataBase.Invoices
                                 where i.User.Email == userEmail && i.Month == month && i.Year == year
                                 select i.Id).FirstOrDefault();
            }

            greeting.InvoiceId = invoice.Id;
            _dataBase.Greetings.Update(greeting);
            await _dataBase.SaveChangesAsync();
        }
    }
}

