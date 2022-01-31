using Microsoft.AspNetCore.Mvc;

namespace GreetingService.API.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    //private static readonly string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    private static readonly Dictionary<int, string> Summary = new()
    {
        {40, "Scorching" },
        {35, "Sweltering" },
        {30, "Hot" },
        {25, "Balmy" },
        {20, "Warm" },
        {15, "Mild" },
        {10, "Cool" },
        { 5, "Chilly" },
        { 0, "Bracing" },
        { -21, "Freezing" }
    };
    private static string GetSummary(int temp)
    {
        foreach (var item in Summary)
        {
            if (temp >= item.Key)
                return item.Value;
        }
        return "Unnatural";
    }
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        int[] temp = new int[5];
        //for (int i = 0; i < 5; i++)
        //{
        //    temp[i] = Random.Shared.Next(-20, 55);
        //}
        foreach (var i in Enumerable.Range(0,4))
            temp[i] = Random.Shared.Next(-20, 55);

        return Enumerable.Range(0,4).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = temp[index],
            Summary = GetSummary(temp[index])

        })
        .ToArray();
    }
}

