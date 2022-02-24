using Microsoft.AspNetCore.Http;

namespace GreetingService.Core.Interfaces;

public interface IAuthHandler
{
    public bool IsAuthorized(HttpRequest req);
}
