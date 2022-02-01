using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreetingService.API.Controllers;

[Route("api/[controller]")]
[BasicAuth]
[ApiController]
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
    public IActionResult Get(Guid id)
    {
        var g = _database.Get(id);
        return (g != null ? Ok(g) : NotFound());
    }


    // POST api/<GreetingController>
    [HttpPost]
    public IActionResult Post([FromBody] Greeting g)
    {
        bool success = _database.Create(g);
        return (success ? Accepted() : Conflict());
    }


    // PUT api/<GreetingController>/5
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Greeting g)
    {
        bool success = _database.Update(g);
        return (success ? Accepted() : NotFound());
    }


    //// DELETE api/<GreetingController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
