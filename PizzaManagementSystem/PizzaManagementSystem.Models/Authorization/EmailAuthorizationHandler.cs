using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PizzaManagementSystem.Models.Interfaces;

namespace PizzaManagementSystem.Models.Authorization;

public class EmailAuthorizationHandler(IUserContext userContext, ILogger<EmailAuthorizationHandler> logger) : AuthorizationHandler<EmailRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailRequirement requirement)
    {
        var currentUser = await userContext.GetCurrentUser();
        logger.LogInformation("currentUser: {user}", currentUser);

        if (currentUser.Email.Contains(requirement.Domain))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}