using Bacheton.Api.Common.Attributes;
using Bacheton.Domain.Constants;

namespace Bacheton.Api.Common.Middlewares;

public class PermissionAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint is null)
        {
            await _next(context);
            return;
        }

        var permissionAttribute = endpoint.Metadata.GetMetadata<RequiredPermissionAttribute>();

        if (permissionAttribute is null)
        {
            await _next(context);
            return;
        }

        var requiredPermission = permissionAttribute.Permission;
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? false)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var permissionsClaim = user.FindFirst(c => c.Type == BachetonConstants.PermissionsClaim)?.Value;

        if (string.IsNullOrEmpty(permissionsClaim))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        
        var permissions = new HashSet<string>(permissionsClaim.Split(","));

        if (!permissions.Contains(requiredPermission) &&
            !permissions.Contains(BachetonConstants.SuperAccessPermission))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        
        await _next(context);
    }
}