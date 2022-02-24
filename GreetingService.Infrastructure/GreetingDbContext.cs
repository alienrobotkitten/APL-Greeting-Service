using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreetingService.Infrastructure;
public class GreetingDbContext : DbContext
{
    public DbSet<Greeting> Greetings { get; set; }
    public DbSet<User> Users { get; set; }
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

    /// <summary>
    /// This is a way to specify table config in db (i.e. specifying primary key)
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Tell EF Core that the primary key of User table is email
        modelBuilder.Entity<User>()
            .HasKey(c => c.Email);
    }
}
