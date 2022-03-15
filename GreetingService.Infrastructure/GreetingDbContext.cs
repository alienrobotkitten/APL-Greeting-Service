using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GreetingService.Infrastructure;
public class GreetingDbContext : DbContext
{
    private readonly IConfiguration _config;

    public DbSet<Greeting> Greetings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public GreetingDbContext(IConfiguration config)
    {
        _config = config;
    }
    public GreetingDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("GreetingDbConnectionString"));
        optionsBuilder.UseSqlServer(_config["GreetingDbConnectionString"]);
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

        //Tell EF Core that Greeting.From is a foreign key for User table. 
        //https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#without-navigation-property
        modelBuilder.Entity<Greeting>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.From)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        //Tell EF Core that Greeting.To is a foreign key for User table
        modelBuilder.Entity<Greeting>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.To)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
