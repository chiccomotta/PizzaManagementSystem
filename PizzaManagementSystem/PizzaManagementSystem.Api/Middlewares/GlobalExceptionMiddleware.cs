using Microsoft.AspNetCore.Mvc;
using PizzaManagementSystem.Models.Authorization;
using System.Net;

namespace PizzaManagementSystem.Api.Middlewares;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotAuthenticatedException ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Detail = ex.Message,
                Status = (int)HttpStatusCode.Unauthorized
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Detail = ex.Message,
                Status = (int)HttpStatusCode.InternalServerError
            });
        }
    }
}