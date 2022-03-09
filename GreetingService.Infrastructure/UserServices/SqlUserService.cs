using GreetingService.Core.Extensions;
using GreetingService.Core.Entities;
using GreetingService.Core.Exceptions;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure.UserServices;

public class SqlUserService : IUserServiceAsync
{
    private readonly IConfiguration _config;
    private readonly ILogger<SqlUserService> _logger;
    private readonly GreetingDbContext _db;

    public SqlUserService(IConfiguration config, ILogger<SqlUserService> logger, GreetingDbContext dbContext)
    {
        _config = config;
        _logger = logger;
        _db = dbContext;
        var canConnect = _db.Database.CanConnect();
        if (!canConnect)
            _logger.LogError("Can't connect to database.");
    }

    public async Task<bool> IsValidUserAsync(string email, string password)
    {
        var user = await _db.Users.FindAsync(email);

        if (user != null)
        {
            return user.Password == password;
        }
        else
        {
            _logger.LogWarning($"User '{email}' not found.");
            throw new UserDoesNotExistException();
        }
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        User? existingUser = await _db.Users.FindAsync(user.Email);
        
        if (existingUser != null)
            //throw new UserAlreadyExistsException(user.Email);
            return false;

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return true;

    }

    public async Task<User> GetUserAsync(string email)
    {
        User? user = await _db.Users.FindAsync(email);

        if (user != null)
            return user;
        else
            throw new UserDoesNotExistException(email);
    }

    public async Task<bool> UpdateUserAsync(User updatedUser)
    {
        User? user = await _db.Users.FindAsync(updatedUser.Email);

        if (user != null)
        {
            await Task.Run(() => _db.Users.Update(updatedUser));
            await _db.SaveChangesAsync();
            return true;
        }
        else
        {
            throw new UserDoesNotExistException(updatedUser.Email);
        }
    }

    public async Task<bool> DeleteUserAsync(string email)
    {
        if (!email.IsValidEmailAddress())
            throw new InvalidEmailException(email);

        User? user = await _db.Users.FindAsync(email);

        if (user != null)
        {
            await Task.Run(() => _db.Users.Remove(user));
            await _db.SaveChangesAsync();
            return true;
        }
        else
        {
            throw new UserDoesNotExistException(email);
        }
    }
}
