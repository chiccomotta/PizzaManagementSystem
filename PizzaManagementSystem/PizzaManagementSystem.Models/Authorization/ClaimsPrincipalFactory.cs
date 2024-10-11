using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PizzaManagementSystem.Models.Models;
using System.Security.Claims;

namespace PizzaManagementSystem.Models.Authorization;

public class ClaimsPrincipalFactory(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<User, IdentityRole>(userManager, roleManager, options)
{
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var identity = await GenerateClaimsAsync(user);

        // Aggiungo il Claim 'Nickname'
        if (!string.IsNullOrWhiteSpace(user.Nickname))
        {
            identity.AddClaim(new Claim("Nickname", user.Nickname));
        }

        // Aggiungere altri claims se necessario

        return new ClaimsPrincipal(identity);
    }
}