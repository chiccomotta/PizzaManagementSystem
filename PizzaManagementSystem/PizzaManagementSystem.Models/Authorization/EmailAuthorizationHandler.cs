using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PizzaManagementSystem.Models.Interfaces;

namespace PizzaManagementSystem.Models.Authorization;

public class EmailAuthorizationHandler(IUserContext userContext, ILogger<EmailAuthorizationHandler> logger) : AuthorizationHandler<EmailRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailRequirement requirement)
    {
        var currentUser = await userContext.GetCurrentUser();
        logger.LogInformation("currentUser: {user}", currentUser);

        if (currentUser.Email.Contains(requirement.Domain, StringComparison.InvariantCultureIgnoreCase))
        {
            context.Succeed(requirement);
            return;
        }

        var reason = new AuthorizationFailureReason(this, "Email is not in the correct Domain");
        context.Fail(reason);

        // Salva il motivo del fallimento nel HttpContext.Items
        if (context.Resource is HttpContext httpContext)
        {
            httpContext.Items["AuthorizationFailureReason"] = reason.Message;
        }
    }
}