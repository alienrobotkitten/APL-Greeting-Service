using Microsoft.AspNetCore.Http;

namespace GreetingService.API.Function.Authentication;

public interface IAuthHandler
{
    public bool IsAuthorized(HttpRequest req);
}
