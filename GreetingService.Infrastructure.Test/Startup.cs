using GreetingService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Serilog;
using Serilog.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using GreetingService.Infrastructure.GreetingRepositories;

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
    //public IHostBuilder CreateHostBuilder([AssemblyName assemblyName]) { }
}
