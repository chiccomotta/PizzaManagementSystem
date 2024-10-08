using Microsoft.AspNetCore.Identity;

namespace PizzaManagementSystem.Models.Models;

public class User : IdentityUser
{
    public string? Nickname { get; set; }
}