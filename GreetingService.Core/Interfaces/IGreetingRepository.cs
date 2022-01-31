using GreetingService.Core.Entities;

namespace GreetingService.Core.Interfaces;

public interface IGreetingRepository
{
    public Greeting Get (Guid id);
    public IEnumerable<Greeting> Get();
    public void Create(Greeting g);
    public void Update(Greeting g);
}
