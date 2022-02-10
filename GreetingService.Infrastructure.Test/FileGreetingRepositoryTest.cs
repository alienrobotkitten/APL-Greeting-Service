using GreetingService.Core.Entities;
using System;
using System.Linq;
using Xunit;

namespace GreetingService.Infrastructure.Test;

[CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
public class FileGreetingRepositoryTest : IClassFixture<RepositoryFixture>
{
       static FileGreetingRepository _repository;

    private static readonly Guid _newGreetingGuid = new Guid("edaa5b6e-c483-4025-b777-7620ec147790");

    private static readonly Guid _wrongGuid = new Guid("b3138c0f-666a-433d-8f9f-426bca7f40a6");

    private static readonly Greeting _newGreeting = new Greeting(
            "Towa",
            "Keen",
            "Message from test of created method",
            _newGreetingGuid,
          new DateTime(2022, 01, 25, 14, 25, 10));

    private static readonly Greeting _updatedGreeting = new Greeting(

            "new sender",
            "new receiver",
            "New message",
            _newGreetingGuid,
            new DateTime(2022, 02, 25, 14, 25, 11));

    public FileGreetingRepositoryTest(RepositoryFixture fixture)
    {
        _repository = fixture.Repository;
    }

    [Fact]
    public void A_GetListEmpty()
    {
        // Get list, should be empty
        var empty_repo = _repository.Get();
        Assert.NotNull(empty_repo);
        Assert.Empty(empty_repo);
    }
    [Fact]
    public void B_Create()
    {

        // Create a greeting, compare to input
        _repository.Create(_newGreeting);
        Assert.NotNull(_repository.Get(_newGreetingGuid));
        var actual_new = _repository.Get(_newGreeting.Id);
        Assert.Equal<Greeting>(_newGreeting, actual_new);
    }
        [Fact]
    public void C_GetWrongId()
    {
        //Try to get wrong guid
        var actual_wrong = _repository.Get(_wrongGuid);
        Assert.Null(actual_wrong);
    }
    [Fact]
    public void D_GetListOne()
    {
        // Get list, should be count 1
        var after_create = _repository.Get();
        Assert.Equal(1, after_create.Count());
    }
    [Fact]
    public void E_Update()
    {

        // Update and compare
        _repository.Update(_updatedGreeting);
        var actual_updated = _repository.Get(_updatedGreeting.Id);
        Assert.Equal<Greeting>(_updatedGreeting, actual_updated);
    }
    [Fact]
    public void F_GetListOne()
    {
        // Get list, should be count 1
        var after_update = _repository.Get();
        Assert.Equal(1, after_update.Count());

    }
    [Fact]
    public void G_Delete()
    {
        // Delete 
        _repository.Delete(_updatedGreeting.Id);
    }
    [Fact]
    public void H_GetListEmpty()
    {   
        // Get list, should be empty
        var after_delete = _repository.Get();
        Assert.Empty(after_delete);
    }
}