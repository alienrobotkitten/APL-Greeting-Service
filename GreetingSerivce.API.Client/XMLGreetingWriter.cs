using System.Xml;
using System.Xml.Serialization;

namespace GreetingService.API.Client;
internal class XMLGreetingWriter
{
    private readonly string _filePath;
    private readonly XmlSerializer _serializer;
    private readonly XmlWriter _xmlWriter;

    internal XMLGreetingWriter()
    {
        _filePath = "../../../greetings.xml";
        var xmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
        };
        //string filename = $"{filePath}Greetings-{DateTime.Now.Ticks}.xml";

        _serializer = new XmlSerializer(typeof(List<Greeting>));

        _xmlWriter = XmlWriter.Create(_filePath, xmlWriterSettings);
    }

    public void Write(List<Greeting> g)
    {
        //convert our greetings of type IEnumerable (interface) to List (concrete class)
        //this xml serializer does not support serializing interfaces, need to convert to a concrete class
        _serializer.Serialize(_xmlWriter, g);                                   

        Console.WriteLine($"Wrote {g.Count()} greeting(s) to {_filePath}");
    }

    public void Write(string XMLformattedString)
    {
       // string currentFilePath = $"{filePath}XMLgreetings-{DateTime.Now.Ticks}.log";
        File.WriteAllText(_filePath, XMLformattedString);
    }

    public void Write(Greeting greeting)
    {
        Write(new List<Greeting>() { greeting });
    }
}


