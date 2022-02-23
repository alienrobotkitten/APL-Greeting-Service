using GreetingService.API.Function.Authentication;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using GreetingService.Infrastructure.GreetingRepositories;
using GreetingService.Infrastructure.UserServices;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = builder.GetContext().Configuration;

        builder.Services.AddHttpClient();

        builder.Services.AddLogging();

        builder.Services.AddScoped<IGreetingRepositoryAsync, SqlGreetingRepository>();

        builder.Services.AddScoped<IUserServiceAsync, BlobUserService>();

        builder.Services.AddScoped<IAuthHandlerAsync, BasicAuthHandlerAsync>();

        builder.Services.AddDbContext<GreetingDbContext>(options =>
        {
            options.UseSqlServer(config["GreetingDbConnectionString"]);
        });

        //Create a Serilog logger and register it as a logger
        //Get the Azure Storage Account connection string from our IConfiguration
        //builder.Services.AddLogging(c =>
        //{
        //    var connectionString = config["LoggingStorageAccount"];
        //    if (string.IsNullOrWhiteSpace(connectionString))
        //        return;

        //    var logName = $"azurefunctionapp.log";
        //    var logger = new LoggerConfiguration()
        //                        .WriteTo.Console()
        //                        .WriteTo.AzureBlobStorage(connectionString,
        //                                                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        //                                                  storageFileName: "{yyyy}/{MM}/{dd}/" + logName,
        //                                                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")                  
        //                        .CreateLogger();

        //    c.AddSerilog(logger, true);
        //});
    }
}
