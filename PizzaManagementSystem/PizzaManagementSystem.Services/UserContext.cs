using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PizzaManagementSystem.Models.Authorization;
using PizzaManagementSystem.Models.Interfaces;
using PizzaManagementSystem.Models.Models;
using System.Security.Claims;

namespace PizzaManagementSystem.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor, ILogger<UserContext> logger, UserManager<User> userManager) : IUserContext
{
    public async Task<CurrentUser> GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("Error user is null or not authenticated");
        }

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r =>r.Value).ToList();
        var languages = user.FindFirst(c => c.Type == ClaimNames.ProgrammingLanguages)?.Value?.Split(',')?.ToList();

        // Get the current logged-in user
        var dbUser = await userManager.GetUserAsync(user);

        // Posso anche prendere i programming languages direttamente dallo User
        // var pl = dbUser.ProgrammingLanguages

        return new CurrentUser(userId, email, dbUser?.Nickname, roles, languages);
    }
}