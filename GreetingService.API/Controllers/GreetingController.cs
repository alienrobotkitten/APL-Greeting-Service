using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreetingService.API.Controllers;

[Route("api/[controller]")]
[BasicAuth]
[ApiController]
[Consumes("application/xml")]
public class GreetingController : ControllerBase
{
    private readonly IGreetingRepository _database;
    public GreetingController(IGreetingRepository database)
    {
        _database = database;
    }
    // GET: api/<GreetingController>
    [HttpGet]
    public IEnumerable<Greeting> Get()
    {
        return _database.Get();
    }

    // GET api/<GreetingController>/5
    [HttpGet("{id}")]
    public Greeting Get(Guid id)
    {
        return _database.Get(id);
    }

    // POST api/<GreetingController>
    [HttpPost]
    public void Post([FromBody] Greeting g)
    {
        _database.Create(g);
    }

    // PUT api/<GreetingController>/5
    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] Greeting g)
    {
        _database.Update(g);
    }

    //// DELETE api/<GreetingController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
