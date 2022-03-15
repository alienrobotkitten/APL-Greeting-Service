namespace GreetingService.Core.Entities;
public enum UserStatus
{
    Pending = 102,  // http status Processing
    Rejected = 406, // http status Not acceptable
    Approved = 202, // http status Accepted
}
