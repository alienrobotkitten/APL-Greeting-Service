using GreetingService.Core.Entities;

public interface IApprovalService
{
	public Task BeginUserApprovalAsync(User user);
}