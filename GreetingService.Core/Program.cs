using System;
using GreetingService.Core;
using GreetingService.Core.Entities;

public class Program
{
    public static void Main()
    {
        string fullJson = new string("{ \"from\": \"Helena\", \"to\": \"Keen\", \"message\": \"This is greeting no 1\", \"id\": \"e2bb33f8-75cc-4e66-8a87-389087e09e1c\", \"timestamp\": \"0001-01-01T00:00:00\" }");
        Greeting g1 = fullJson.ToGreeting();

        string fourJson = new string("{ \"from\": \"Helena\", \"to\": \"Keen\", \"message\": \"This is greeting no 1\" }");
        var g2 = fourJson.ToGreeting();

        string threeJson = new string("{ \"from\": \"Helena\", \"to\": \"Keen\", \"message\": \"This is greeting no 1\", \"id\": \"e2bb33f8-75cc-4e66-8a87-389087e09e1c\" }");
        var g3 = threeJson.ToGreeting();    

        string threemoreJson = new string("{ \"from\": \"Helena\", \"to\": \"Keen\", \"message\": \"This is greeting no 1\", \"timestamp\": \"0001-01-01T00:00:00\" }");
        var g4 = threemoreJson.ToGreeting();
    }
}
