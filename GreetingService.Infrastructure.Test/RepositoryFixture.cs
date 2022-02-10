using GreetingService.Infrastructure;
using System.IO;

public class RepositoryFixture
{
    public FileGreetingRepository Repository { get; set; }
    private const string _filename = "../../../testdata/TestGreetings.json";

    public RepositoryFixture()
    {
        if (File.Exists(_filename))
            File.Delete(_filename);

        Repository = new FileGreetingRepository(_filename);
    }
}