﻿using GreetingService.Core.Exceptions;
using GreetingService.Core.Extensions;

namespace GreetingService.Core.Entities;
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            bool isValid = value.IsValidEmailAddress();
            if (!isValid)
                throw new InvalidEmailException(value);
            _email = value;
        }
    }
    public string Password { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public UserStatus ApprovalStatus { get; set; }
    public string ApprovalStatusNote { get; set; }
    public string ApprovalCode { get; set; }
    public DateTime ApprovalExpiry { get; set; }

    public User()
    {

    }

}
