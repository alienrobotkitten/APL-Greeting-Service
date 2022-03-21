using GreetingService.API.Function.Authentication;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using GreetingService.Infrastructure.GreetingRepositories;
using GreetingService.Infrastructure.InvoiceServices;
using GreetingService.Infrastructure.MessagingServices;
using GreetingService.Infrastructure.UserServices;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using Microsoft.Extensions.Azure;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        var vaultUri = System.Environment.GetEnvironmentVariable("KeyVaultUri");
        //var azureServiceTokenProvider = new AzureServiceTokenProvider();
        //var keyVaultClient = new KeyVaultClient(
        //    new KeyVaultClient.AuthenticationCallback(
        //        azureServiceTokenProvider.KeyVaultTokenCallback));
        //builder.ConfigurationBuilder.AddAzureKeyVault(
        //    vaultUri, keyVaultClient, new DefaultKeyVaultSecretManager());

        //try
        //{
        //    builder.ConfigurationBuilder.AddAzureKeyVault(vaultUri);
        //}
        //catch (Exception e)
        //{
        //    Console.Write(e.Message);
        //}
        try
        {
            builder.ConfigurationBuilder.AddAzureKeyVault(vaultUri, new DefaultKeyVaultSecretManager());
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = builder.GetContext().Configuration;

        builder.Services.AddHttpClient();

        builder.Services.AddLogging();

        builder.Services.AddDbContext<GreetingDbContext>(options =>
        {
            options.UseSqlServer(System.Environment.GetEnvironmentVariable("GreetingDbConnectionString"));
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(config["ServiceBusConnectionString"]);
        });

        builder.Services.AddControllers();

        builder.Services.AddScoped<IAuthHandlerAsync, BasicAuthHandlerAsync>();
        builder.Services.AddScoped<IGreetingRepositoryAsync, CosmosDbGreetingRepository>();
        builder.Services.AddScoped<IInvoiceService, SqlInvoiceService>();
        builder.Services.AddScoped<IUserServiceAsync, SqlUserService>();
        builder.Services.AddSingleton<IMessagingService, ServiceBusMessagingService>();
        builder.Services.AddScoped<IApprovalService, TeamsApprovalService>();
  

        //Create a Serilog logger and register it as a logger
        //Get the Azure Storage Account connection string from our IConfiguration
        builder.Services.AddLogging(c =>
        {
            var connectionString = config["LoggingStorageAccount"];
            if (string.IsNullOrWhiteSpace(connectionString))
                return;

            var logName = $"azurefunctionapp.log";
            var logger = new LoggerConfiguration()
                                .WriteTo.Console()
                                //.WriteTo.AzureBlobStorage(connectionString,
                                //                          restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                //                          storageFileName: "{yyyy}/{MM}/{dd}/" + logName,
                                //                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
                                .CreateLogger();

            c.AddSerilog(logger, true);
        });
    }
}
