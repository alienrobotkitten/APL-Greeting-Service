using GreetingService.Core.Extensions;
using System.Security.Cryptography;

//var random = new Random();
//string[] names = { "kajsa@ankeborg.com", "kalle@ankeborg.com","musse@ankeborg.com" };
//string[] messages = { "Hello", "Hi there!", "Good evening!", "Long time no see!", "Happy new year!", "Merry Xmas!"};
//for (int i = 0; i < 10; i++)
//{
//    int year = random.Next(2020,2022);
//    int month = Random.Shared.Next(1,13);
//    int day = random.Next(1,29);
//    int hour = random.Next(0,24);
//    int minute = random.Next(0,60);
//    int second = random.Next(0,60);
//    var d = new DateTime(year, month, day, hour, minute, second);
//    Console.WriteLine($"{d.ToString()}");
//    names.GetRandom();
//}

static string GetRandomAlphanumericString(int length)
{
    const string alphanumericCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz" +
        "0123456789";
    return GetRandomString(length, alphanumericCharacters);
}


static string GetRandomString(int length, IEnumerable<char> characterSet)
{
    if (length < 0)
        throw new ArgumentException("length must not be negative", "length");
    if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
        throw new ArgumentException("length is too big", "length");
    if (characterSet == null)
        throw new ArgumentNullException("characterSet");
    var characterArray = characterSet.Distinct().ToArray();
    if (characterArray.Length == 0)
        throw new ArgumentException("characterSet must not be empty", "characterSet");

    var bytes = new byte[length * 8];
    var result = new char[length];
    using (var cryptoProvider = new RNGCryptoServiceProvider())
    {
        cryptoProvider.GetBytes(bytes);
    }
    for (int i = 0; i < length; i++)
    {
        ulong value = BitConverter.ToUInt64(bytes, i * 8);
        result[i] = characterArray[value % (uint)characterArray.Length];
    }
    return new string(result);
}