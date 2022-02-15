using GreetingService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace GreetingService.Infrastructure.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGreetingRepository,MemoryGreetingRepository>();
        services.AddSingleton<ILogger, Logger>();
    }
    //public IHostBuilder CreateHostBuilder([AssemblyName assemblyName]) { }
}
