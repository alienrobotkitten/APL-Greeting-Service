using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace GreetingService.API.Function.Authentication;

internal class BasicAuthHandler : IAuthHandler
{
    private readonly ILogger<BasicAuthHandler> _log;
    private readonly IUserService _iuserservice;

    public BasicAuthHandler(IUserService iuserservice, ILogger<BasicAuthHandler> log)
    {
        _iuserservice = iuserservice;
        _log = log;
    }
    public bool IsAuthorized(HttpRequest req)
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

                        if (IsAuthorized(user, password))
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

    public bool IsAuthorized(string username, string password)
    {
        return _iuserservice.IsValidUser(username, password);
    }
}
