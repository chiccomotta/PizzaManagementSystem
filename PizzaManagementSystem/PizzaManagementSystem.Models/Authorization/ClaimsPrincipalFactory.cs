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
            identity.AddClaim(new Claim(ClaimNames.Nickname, user.Nickname));
        }

        // Aggiungo il Claim 'ProgrammingLanguages'
        if (!string.IsNullOrWhiteSpace(user.ProgrammingLanguages))
        {
            identity.AddClaim(new Claim(ClaimNames.ProgrammingLanguages, user.ProgrammingLanguages));
        }

        return new ClaimsPrincipal(identity);
    }
}