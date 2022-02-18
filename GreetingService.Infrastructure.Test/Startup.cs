using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure.GreetingRepositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GreetingService.Infrastructure.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IGreetingRepository,FileGreetingRepository>();
        services.AddLogging(c =>
        {
            var logName = $"azurefunctionapp.log";
            var logger = new LoggerConfiguration()
                                .CreateLogger();

            c.AddSerilog(logger, true);
        });
    }
}
