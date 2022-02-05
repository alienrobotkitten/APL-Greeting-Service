using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddLogging()<Serilog.ILogger, Serilog.Core.Logger>(env =>
//{
//    return new LoggerConfiguration()
//                      .WriteTo.Console()
//                      .CreateLogger();
//});
builder.Services.AddSingleton<Serilog.ILogger, Serilog.Core.Logger>(env =>
 {
     return new Serilog.LoggerConfiguration()
                       .WriteTo.File("./data/serilog.log")
                       .CreateLogger();
 });
builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(env =>
{
    //IConfiguration? appsettings = env.GetService<IConfiguration>();
    return new FileGreetingRepository("./data/NewGreetings.json");
});

builder.Services.AddScoped<IUserService, AppSettingsUserService>();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
