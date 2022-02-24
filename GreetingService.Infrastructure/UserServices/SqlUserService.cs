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

    public async Task<bool> IsValidUserAsync(string username, string password)
    {
        var user = await _db.Users.FindAsync(username);

        if (user != null)
        {
            return user.Password == password;
        }
        else
        {
            _logger.LogWarning($"User '{username}' not found.");
            return false;
        }
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        try
        {
            _db.Users.Add(user);
            await Task.Run(() => _db.Users.Add(user));
            await Task.Run(() => _db.SaveChanges());
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<User> GetUserAsync(string email)
    {
        User? user = await Task.Run(() => _db.Users.FirstOrDefault(u => u.Email == email));
        return user;
    }

    public async Task<bool> UpdateUserAsync(User updatedUser)
    {
        User? user = await Task.Run(() => _db.Users.FirstOrDefault(u => u.Email == updatedUser.Email));
        if (user != null)
        {
            await Task.Run(() => _db.Users.Update(updatedUser));
            await Task.Run(() => _db.SaveChanges());
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string email)
    {
        User? user = await Task.Run(() => _db.Users.FirstOrDefault(u => u.Email == email));

        if (user != null)
        {
            await Task.Run(() => _db.Users.Remove(user));
            return true;
        }
        else
        {
            return false;
        }
    }
}
