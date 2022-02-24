using Microsoft.AspNetCore.Http;

namespace GreetingService.Core.Interfaces;

public interface IAuthHandlerAsync
{
    public Task<bool> IsAuthorizedAsync(HttpRequest req);
}
