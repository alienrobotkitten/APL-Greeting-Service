using GreetingService.Core.Entities;

namespace GreetingService.Core.Interfaces;

public interface IUserServiceAsync
{
    public Task<bool> IsValidUserAsync(string username, string password);
    public Task<bool> CreateUserAsync(User user);
    public Task<User> GetUserAsync(string email);
    public Task<bool> UpdateUserAsync(User user);
    public Task<bool> DeleteUserAsync(string email);
    public Task ApproveUserAsync(string approvalCode);
    public Task RejectUserAsync(string approvalCode);
}
