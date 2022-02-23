using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GreetingService.Infrastructure.GreetingRepositories;

public class SqlGreetingRepository : IGreetingRepositoryAsync
{
    private GreetingDbContext _greetingDbContext { get; set; }

    private ILogger<SqlGreetingRepository> _log;

    public SqlGreetingRepository(GreetingDbContext greetingDbContext, ILogger<SqlGreetingRepository> log)
    {
        _greetingDbContext = greetingDbContext;
        _log = log;
    }

    public async Task<bool> CreateAsync(Greeting g)
    {
        try
        {
            await _greetingDbContext.AddAsync<Greeting>(g);
            await _greetingDbContext.SaveChangesAsync();
            _log.LogInformation($"Successfully added {g.Id} to database.");
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError($"Error when trying to add greeting with id {g.Id}: {ex}");
            return false;
        }

    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Greeting gr = await Task.Run(() => _greetingDbContext.Greetings.FirstOrDefault(g => g.Id == id));
        try
        {
            if (gr != null)
            {
                await Task.Run(() => _greetingDbContext.Greetings.Remove(gr));
                await _greetingDbContext.SaveChangesAsync();
                _log.LogInformation($"Successfully deleted {id} from database.");
                return true;
            }
            else
            {
                _log.LogInformation($"No greeting with {id} in database.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _log.LogError($"Error when trying to delete greeting with id {id}: {ex}");
            return false;
        }
    }

    public async Task<Greeting>? GetAsync(Guid id)
    {
        try
        {
            Greeting gr = await Task.Run(() => _greetingDbContext.Greetings.FirstOrDefault(g => g.Id == id));
            _log.LogInformation($"Successfully retrieved {id} from database.");
            return gr;
        }
        catch (Exception ex)
        {
            _log.LogError($"Error when trying to retrieve greeting with id {id}: {ex}");
            return null;
        }
    }

    public async Task<IEnumerable<Greeting>> GetAsync()
    {
        List<Greeting> greetings = await Task.Run(() => _greetingDbContext.Greetings.ToList());
        return greetings;
    }

    public async Task<bool> UpdateAsync(Greeting updatedGreeting)
    {
        try
        {
            Greeting greetingToRemove = await Task.Run(() => _greetingDbContext.Greetings.FirstOrDefault(g => g.Id == updatedGreeting.Id));
            if (greetingToRemove != null)
            {
                await Task.Run(() => _greetingDbContext.Remove(greetingToRemove));
                await _greetingDbContext.AddAsync<Greeting>(updatedGreeting);
                await _greetingDbContext.SaveChangesAsync();
                _log.LogInformation($"Successfully updated {updatedGreeting.Id}.");
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _log.LogError($"Error when trying to update greeting with id {updatedGreeting.Id}: {ex}");

            return false;
        }
    }
}