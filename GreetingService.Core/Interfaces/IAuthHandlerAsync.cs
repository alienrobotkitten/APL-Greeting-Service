using Microsoft.AspNetCore.Http;

namespace GreetingService.API.Function.Authentication;

public interface IAuthHandlerAsync
{
    public Task<bool> IsAuthorizedAsync(HttpRequest req);
}
