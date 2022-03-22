using GreetingService.Core.Entities;
using GreetingService.Core.Exceptions;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure.GreetingRepositories;

public class SqlGreetingRepository : IGreetingRepositoryAsync
{
    private GreetingDbContext _greetingDbContext { get; set; }
    private ILogger<SqlGreetingRepository> _log;

    public SqlGreetingRepository(GreetingDbContext greetingDbContext, ILogger<SqlGreetingRepository> log)
    {
        _greetingDbContext = greetingDbContext;
        _log = log;
        var canConnect = _greetingDbContext.Database.CanConnect();
        if (!canConnect)
            _log.LogError("Can't connect to database.");
    }

    public async Task<bool> CreateAsync(Greeting g)
    {
        g.To = g.To.Trim().ToLower();
        g.From = g.From.Trim().ToLower();

        try
        {
            User toUser = await _greetingDbContext.Users.FindAsync(g.To);
            if (toUser == null)
                throw new UserDoesNotExistException(g.To);

            User fromUser = await _greetingDbContext.Users.FindAsync(g.From);
            if (fromUser == null)
                throw new UserDoesNotExistException(g.From);

            await _greetingDbContext.Greetings.AddAsync(g);
            await _greetingDbContext.SaveChangesAsync();

            _log.LogInformation($"Successfully added {g.Id} to database.");
            return true;
        }
        catch (Exception e) when (e is UserDoesNotExistException or InvalidEmailException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _log.LogError($"Error when trying to add greeting with id {g.Id}: {ex}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            Greeting gr = await _greetingDbContext.Greetings.FindAsync(id);
            if (gr != null)
            {
                await Task.Run(() => _greetingDbContext.Greetings.Remove(gr));
                await _greetingDbContext.SaveChangesAsync();
                _log.LogInformation($"Successfully deleted {id} from database.");
                return true;
            }
            else
            {
                throw new GreetingNotFoundException(id.ToString());
            }
        }
        catch (Exception e) when (e is UserDoesNotExistException or InvalidEmailException or GreetingNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when trying to delete greeting with id {id}.", ex);
        }
    }

    public async Task<Greeting>? GetAsync(Guid id)
    {
        try
        {
            Greeting gr = await _greetingDbContext.Greetings.FindAsync(id);
            if (gr == null)
                throw new GreetingNotFoundException(id.ToString());
            _log.LogInformation($"Successfully retrieved {id} from database.");
            return gr;
        }
        catch (GreetingNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when trying to retrieve greeting with id {id}.", ex);
        }
    }

    public async Task<IEnumerable<Greeting>> GetAsync()
    {
        var greetings = await Task.Run(() => _greetingDbContext.Greetings.ToList<Greeting>());
        return greetings;
    }

    public async Task<bool> UpdateAsync(Greeting g)
    {
        g.To = g.To.Trim().ToLower();
        g.From = g.From.Trim().ToLower();
        try
        {
            Greeting greetingToRemove = await _greetingDbContext.Greetings.FindAsync(g.Id);
            if (greetingToRemove != null)
            {
                User toUser = await _greetingDbContext.Users.FindAsync(g.To);
                if (toUser == null)
                    throw new UserDoesNotExistException(g.To);

                User fromUser = await _greetingDbContext.Users.FindAsync(g.From);
                if (fromUser == null)
                    throw new UserDoesNotExistException(g.From);

                if (greetingToRemove.InvoiceId != g.InvoiceId)
                    throw new Exception("Invoice id doesn't match. Update will not be performed.");

                await Task.Run(() => _greetingDbContext.Greetings.Remove(greetingToRemove));
                await _greetingDbContext.Greetings.AddAsync(g);
                await _greetingDbContext.SaveChangesAsync();
                _log.LogInformation($"Successfully updated {g.Id}.");
                return true;
            }
            else
            {
                throw new GreetingNotFoundException(g.Id.ToString());
            }
        }
        catch (Exception ex)
        {
            _log.LogError(ex.ToString());
            return false;
        }
    }

    public async Task<IEnumerable<Greeting>> GetAsync(string? fromUser = null, string? toUser = null)
    {
        List<Greeting> greetings;

        if (toUser != null)
            toUser = toUser.Trim().ToLower();
        
        if (fromUser != null)
            fromUser = fromUser.Trim().ToLower();

        if (fromUser != null && toUser != null)
        {
            var query = await Task.Run(() =>
                            from g in _greetingDbContext.Greetings
                            where g.To == toUser && g.From == fromUser
                            select g
                        );
            greetings = query.ToList();

        }
        else if (fromUser != null && toUser == null)
        {
            var query = await Task.Run(() =>
                            from g in _greetingDbContext.Greetings
                            where g.From == fromUser
                            select g
                        );
            greetings = query.ToList();
        }
        else if (fromUser == null && toUser != null)
        {
            var query = await Task.Run(() =>
                            from g in _greetingDbContext.Greetings
                            where g.To == toUser
                            select g
                        );
            greetings = query.ToList();
        }
        else
        {
            greetings = await Task.Run(() => _greetingDbContext.Greetings.ToList());
        }
        return greetings;
    }
}