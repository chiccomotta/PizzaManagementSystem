using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PizzaManagementSystem.Api.Middlewares;

public class AuthorizationFailureMiddleware(RequestDelegate next, ILogger<AuthorizationFailureMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        await next(context);

        // Verifica se il codice di stato è Forbidden (403)
        if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            // Controlla se c'è un messaggio di fallimento nell'HttpContext.Items
            if (context.Items.ContainsKey("AuthorizationFailureReason"))
            {
                var failureReason = context.Items["AuthorizationFailureReason"] as string;
                logger.LogWarning("Authorization failed: {failureReason}", failureReason);

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Detail = failureReason,
                    Status = (int)HttpStatusCode.Forbidden
                });
            }
        }
    }
}