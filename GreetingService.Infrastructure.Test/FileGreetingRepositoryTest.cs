using GreetingService.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace GreetingService.Infrastructure.Test;
public class FileGreetingRepositoryTest
{
    private const string _filename = "../../../testdata/TestGreetings.json";
    private static FileGreetingRepository _repository;

    private static readonly string _jsonGreetings = "[\n    {\n        \"Timestamp\": \"0001-01-01T00:00:00\",\n        \"Message\": \"This is greeting no 0\",\n        \"From\": \"Helena\",\n        \"To\": \"\",\n        \"Id\": \"82dc8be9-fb57-4a61-bdf9-6bdd9383ebcf\"\n    },\n    {\n        \"Timestamp\": \"0001-01-01T00:00:00\",\n        \"Message\": \"This is greeting no 1\",\n        \"From\": \"\",\n        \"To\": \"\",\n        \"Id\": \"d0a59a9f-68a3-42f0-90f0-0d2c30fd3a07\"\n    },\n    {\n        \"Timestamp\": \"0001-01-01T00:00:00\",\n        \"Message\": \"This is greeting no 2\",\n        \"From\": \"\",\n        \"To\": \"\",\n        \"Id\": \"829aadca-9dab-4bb4-b558-0013b6bb9071\"\n    },\n    {\n        \"Timestamp\": \"0001-01-01T00:00:00\",\n        \"Message\": \"This is greeting no 3\",\n        \"From\": \"\",\n        \"To\": \"\",\n        \"Id\": \"f6a457d4-3a71-4986-86e3-cfcef6bb9b70\"\n    },\n    {\n        \"Timestamp\": \"0001-01-01T00:00:00\",\n        \"Message\": \"This is greeting no 4\",\n        \"From\": \"\",\n        \"To\": \"\",\n        \"Id\": \"c3641837-362b-43ef-85c8-3b59e5b50380\"\n    }\n]";

    private static readonly List<Guid> _guids = new()
    {
        new Guid(
            "82dc8be9-fb57-4a61-bdf9-6bdd9383ebcf"
        ),
        new Guid(
            "d0a59a9f-68a3-42f0-90f0-0d2c30fd3a07"
        ),
        new Guid(
            "829aadca-9dab-4bb4-b558-0013b6bb9071"
        ),
        new Guid(
            "f6a457d4-3a71-4986-86e3-cfcef6bb9b70"
        ),
        new Guid(
            "c3641837-362b-43ef-85c8-3b59e5b50380"
        )
    };

    private static readonly Guid _newGreetingGuid = new Guid(
                   "edaa5b6e-c483-4025-b777-7620ec147790"
               );

    private static readonly Greeting _newGreeting = new Greeting(
            new DateTime(2022, 01, 25, 14, 25, 10),
            "Towa",
            "Keen",
            "Message from test of created method",
            _newGreetingGuid);

    private static readonly Greeting _updatedGreeting = new Greeting(
            new DateTime(2022, 02, 25, 14, 25, 11),
            "new sender",
            "new receiver",
            "New message",
            _guids.ElementAt(3));

    private static void Init()
    {
        if (File.Exists(_filename))
            File.Delete(_filename);
        _repository = new FileGreetingRepository(_filename);
        List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(
        _jsonGreetings);

        foreach (var g in greetings)
            _repository.Create(g);
    }

    [Fact]
    public void Get()
    {
        Init();

        var greetings = _repository.Get();

        Assert.NotNull(greetings);
        Assert.NotEmpty(greetings);
        Assert.Equal(5, greetings.Count());
    }
    [Fact]
    public void Get_Guid()
    {
        Init();

        // Happy path
        foreach (var guid in _guids)
            Assert.NotNull(_repository.Get(guid));

        // No such id
//        Assert.Throws<Exception>(() => _repository.Get(_newGreeting.Id));
    }
    [Fact]
    public void Create()
    {
        Init();

        _repository.Create(_newGreeting);

        var actual = _repository.Get(_newGreeting.Id);
        Assert.Equal(_newGreeting, actual);

        List<Greeting> greetings = _repository.Get().ToList();
        Assert.Equal(6, greetings.Count());
    }
    [Fact]
    public void Update()
    {
        Init();

        _repository.Update(_updatedGreeting);

        var actual = _repository.Get(_updatedGreeting.Id);

        Assert.Equal<Greeting>(_updatedGreeting, actual);
    }
}