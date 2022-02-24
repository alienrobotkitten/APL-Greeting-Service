namespace GreetingService.Core.Entities;
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
  
    public User()
    {

    }
}
