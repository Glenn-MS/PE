using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformOrchestrator.Api.Middleware;

/// <summary>
/// Middleware for enforcing role-based access control (RBAC)
/// </summary>
public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;

    public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

        if (user == null || !user.Identity?.IsAuthenticated == true)
        {
            _logger.LogWarning("Unauthorized access attempt");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        // Check for required roles (e.g., Admin, Developer)
        var requiredRoles = new[] { "Admin", "Developer" };
        var userRoles = user.Claims.Where(c => c.Type == "roles").Select(c => c.Value);

        if (!requiredRoles.Any(role => userRoles.Contains(role)))
        {
            _logger.LogWarning("Access denied for user with roles: {UserRoles}", string.Join(", ", userRoles));
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access denied");
            return;
        }

        await _next(context);
    }
}
