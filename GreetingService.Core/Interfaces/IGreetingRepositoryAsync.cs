using GreetingService.Core.Entities;

namespace GreetingService.Core.Interfaces;

public interface IGreetingRepositoryAsync
{
    public Task<Greeting>? GetAsync(string greetingName);
    public Task<IEnumerable<Greeting>> GetAsync(string from, string to);
    public Task<IEnumerable<Greeting>> GetAsync();
    public Task<bool> CreateAsync(Greeting g);
    public Task<bool> UpdateAsync(Greeting g);
    public Task<bool> DeleteAsync(string greetingName);
}
