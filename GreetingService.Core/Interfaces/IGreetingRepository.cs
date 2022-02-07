﻿using GreetingService.Core.Entities;

namespace GreetingService.Core.Interfaces;

public interface IGreetingRepository
{
    public Greeting? Get(Guid id);
    public IEnumerable<Greeting> Get();
    public bool Create(Greeting g);
    public bool Update(Greeting g);
    bool Delete(Guid id);
}
