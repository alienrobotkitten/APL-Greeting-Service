using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreetingService.Infrastructure;
public class GreetingDbContext : DbContext
{
    public DbSet<Greeting> Greetings { get; set; }

    public GreetingDbContext()
    {
    }
    public GreetingDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("GreetingDbConnectionString"));
    }
}
