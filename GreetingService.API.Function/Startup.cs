using GreetingService.API.Function.Authentication;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        IConfiguration config = builder.GetContext().Configuration;

        builder.Services.AddHttpClient();

        builder.Services.AddLogging();

        builder.Services.AddSingleton<IGreetingRepository, FileGreetingRepository>();

        builder.Services.AddScoped<IUserService, HardCodedUserService>();

        builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();
    }
}
