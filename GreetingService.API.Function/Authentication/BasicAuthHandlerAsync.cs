using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.API.Function.Authentication;

internal class BasicAuthHandlerAsync : IAuthHandlerAsync
{
    private readonly ILogger<BasicAuthHandlerAsync> _log;
    private readonly IUserServiceAsync _iuserservice;

    public BasicAuthHandlerAsync(IUserServiceAsync iuserservice, ILogger<BasicAuthHandlerAsync> log)
    {
        _iuserservice = iuserservice;
        _log = log;
    }
    public async Task<bool> IsAuthorizedAsync(HttpRequest req)
    {
        try
        {
            string authHeader = req.Headers["Authorization"];

            if (authHeader != null)
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var credentials = Encoding.UTF8
                                        .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                                        .Split(':', 2);

                    if (credentials.Length == 2)
                    {
                        var user = credentials[0];
                        var password = credentials[1];

                        if (await IsAuthorizedAsync(user, password))
                        {
                            _log.LogInformation($"User {user} authenticated successfully.");
                            return true;
                        }
                        else
                        {
                            _log.LogWarning("$User {user} did not provide the correct password.");
                            return false;
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message);
        }
        return false;
    }

    public async Task<bool> IsAuthorizedAsync(string username, string password)
    {
        return await _iuserservice.IsValidUserAsync(username, password);
    }
}
