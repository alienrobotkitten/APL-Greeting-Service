using GreetingService.Core.Entities;
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
            return false;
        }
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        try
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<User> GetUserAsync(string email)
    {
        User? user = await _db.Users.FindAsync(email);
        return user;
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
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string email)
    {
        User? user = await _db.Users.FindAsync(email);

        if (user != null)
        {
            await Task.Run(() => _db.Users.Remove(user));
            await _db.SaveChangesAsync();   
            return true;
        }
        else
        {
            return false;
        }
    }
}
