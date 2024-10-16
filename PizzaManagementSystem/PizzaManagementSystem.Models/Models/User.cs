using Microsoft.AspNetCore.Identity;

namespace PizzaManagementSystem.Models.Models;

public class User : IdentityUser
{
    public string? Nickname { get; set; }
    public DateTime BirthDay { get; set; }
    public string? ProgrammingLanguages { get; set; }
}