using GreetingService.API.Function.Authentication;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        IConfiguration config = builder.GetContext().Configuration;

        builder.Services.AddHttpClient();
        builder.Services.AddLogging();

        //Create a Serilog logger and register it as a logger
        //Get the Azure Storage Account connection string from our IConfiguration
        builder.Services.AddLogging(c =>
        {

            var connectionString = config["LoggingStorageAccount"];
            if (string.IsNullOrWhiteSpace(connectionString))
                return;

            var logName = $"azurefunctionapp.log";
            var logger = new LoggerConfiguration()
                                .WriteTo.AzureBlobStorage(connectionString,
                                                          restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                                          storageFileName: "{yyyy}/{MM}/{dd}/" + logName,
                                                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")                  
                                .CreateLogger();

            c.AddSerilog(logger, true);
        });

        builder.Services.AddSingleton<IGreetingRepository, FileGreetingRepository>();

        builder.Services.AddScoped<IUserService, HardCodedUserService>();

        builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();
    }
}
