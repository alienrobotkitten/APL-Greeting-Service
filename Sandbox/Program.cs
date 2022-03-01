using GreetingService.Core.Extensions;

var random = new Random();
string[] names = { "kajsa@ankeborg.com", "kalle@ankeborg.com","musse@ankeborg.com" };
string[] messages = { "Hello", "Hi there!", "Good evening!", "Long time no see!", "Happy new year!", "Merry Xmas!"};
for (int i = 0; i < 10; i++)
{
    int year = random.Next(2020,2022);
    int month = Random.Shared.Next(1,13);
    int day = random.Next(1,29);
    int hour = random.Next(0,24);
    int minute = random.Next(0,60);
    int second = random.Next(0,60);
    var d = new DateTime(year, month, day, hour, minute, second);
    Console.WriteLine($"{d.ToString()}");
    names.GetRandom();
}

