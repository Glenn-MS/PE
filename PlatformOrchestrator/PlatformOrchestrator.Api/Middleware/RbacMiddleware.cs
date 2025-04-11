using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlatformOrchestrator.Api.Middleware;

public class RbacMiddleware
{
    private readonly RequestDelegate _next;

    public RbacMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var requiredRoles = endpoint.Metadata.GetMetadata<RolesAttribute>()?.Roles;
            if (requiredRoles != null && requiredRoles.Any())
            {
                var userRoles = context.User.Claims
                    .Where(c => c.Type == "roles") // Entra ID uses 'roles' claim for role assignments
                    .Select(c => c.Value);

                if (!requiredRoles.Intersect(userRoles).Any())
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access denied: insufficient permissions.");
                    return;
                }
            }
        }

        await _next(context);
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RolesAttribute : Attribute
{
    public string[] Roles { get; }

    public RolesAttribute(params string[] roles)
    {
        Roles = roles;
    }
}
