using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using System;
using System.Linq;
using Xunit;

namespace GreetingService.Infrastructure.Test;

public class GreetingRepositoryTest //: IClassFixture<RepositoryFixture>
{
    private static IGreetingRepository _repository;

    private static readonly Guid _newGreetingGuid = new Guid("edaa5b6e-c483-4025-b777-7620ec147790");

    private static readonly Guid _wrongGuid = new Guid("b3138c0f-666a-433d-8f9f-426bca7f40a6");

    private static readonly Greeting _newGreeting = new Greeting(
            "kalle@ankeborg.com",
            "kajsa@ankeborg.com",
            "Message from test of created method",
            _newGreetingGuid,
          new DateTime(2022, 01, 25, 14, 25, 10));

    private static readonly Greeting _updatedGreeting = new Greeting(

            "kalle@ankeborg.com",
            "kajsa@ankeborg.com",
            "New message",
            _newGreetingGuid,
            new DateTime(2022, 02, 25, 14, 25, 11));

    public GreetingRepositoryTest(IGreetingRepository repo)
    {
        _repository = repo;
    }

    [Fact]
    public void GetGreetings()
    {
        // Get list, should be empty
        var empty_repo = _repository.Get();
        Assert.NotNull(empty_repo);
    }
    [Fact]
    public void CreateGreeting()
    {
        // Create a greeting, compare to input
        _repository.Create(_newGreeting);

        Assert.NotNull(_repository.Get(_newGreetingGuid));
        var actual_new = _repository.Get(_newGreeting.Id);
        Assert.Equal<Greeting>(_newGreeting, actual_new);
    }
    [Fact]
    public void GetWrongId()
    {
        _repository.Create(_newGreeting);

        //Try to get wrong guid
        var actual_wrong = _repository.Get(_wrongGuid);
        Assert.Null(actual_wrong);
    }
    [Fact]
    public void GetListOne()
    {
        _repository.Create(_newGreeting);

        // Get list, should be count 1
        var after_create = _repository.Get();
        Assert.Equal(1, after_create.Count());
    }
    [Fact]
    public void UpdateGreeting()
    {
        _repository.Create(_newGreeting);
        // Update and compare
        _repository.Update(_updatedGreeting);

        var actual_updated = _repository.Get(_updatedGreeting.Id);
        Assert.Equal<Greeting>(_updatedGreeting, actual_updated);
        Assert.NotEqual<Greeting>(_newGreeting, actual_updated);
    }
    
    [Fact]
    public void DeleteGreeting()
    {
        _repository.Create(_newGreeting);

        // Delete 
        _repository.Delete(_newGreetingGuid);
        var actual = _repository.Get(_newGreetingGuid);
        Assert.Null(actual);
    }
}