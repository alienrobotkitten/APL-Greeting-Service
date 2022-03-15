using System.Security.Cryptography;

namespace GreetingService.Core;
public class ApprovalCodeGenerator
{
    public static string GetApprovalCode()
    {
        return GetRandomAlphanumericString(42);
    }
    public static string GetRandomAlphanumericString(int length)
    {
        const string alphanumericCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789";
        return GetRandomString(length, alphanumericCharacters);
    }

    private static string GetRandomString(int length, IEnumerable<char> characterSet)
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

        var result = new char[length];
        var bytes = RandomNumberGenerator.GetBytes(length*8);
        
        for (int i = 0; i < length; i++)
        {
            ulong value = BitConverter.ToUInt64(bytes, i * 8);
            result[i] = characterArray[value % (uint)characterArray.Length];
        }
        return new string(result);
    }
}
