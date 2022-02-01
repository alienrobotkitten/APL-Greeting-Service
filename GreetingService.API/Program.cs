using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;

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
builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(env =>
{
    IConfiguration? appsettings = env.GetService<IConfiguration>();
    return new FileGreetingRepository(appsettings?["FileGreetingReposityPath"] ?? "");
});

builder.Services.AddScoped<IUserService, HardCodedUserService>();

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
