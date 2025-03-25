using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Constants;
using Bacheton.Domain.Entities;
using Bacheton.Domain.Errors;
using ErrorOr;

namespace Bacheton.Api.Utilities;

public class AuthUtilities : IAuthUtilities
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IModuleRepository _moduleRepository;

    public AuthUtilities(IHttpContextAccessor httpContextAccessor, IConfiguration config,
        IUserRepository userRepository, IPermissionRepository permissionRepository, IModuleRepository moduleRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _config = config;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _moduleRepository = moduleRepository;
    }

    public void SetRefreshToken(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Authentication:RefreshTokenExpireDays")),
            SameSite = SameSiteMode.None,
            Secure = true
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var claim = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(claim, out var userId)
            ? userId
            : throw new AuthenticationException("Usuario no autenticado");
    }

    public bool HasSuperAccess()
    {
        var claim = _httpContextAccessor.HttpContext?.User?
            .Claims.FirstOrDefault(c => c.Type == BachetonConstants.PermissionsClaim)?.Value;

        return claim?.Contains(BachetonConstants.Permissions.SuperAccessPermission) ?? false;
    }

    public async Task<ErrorOr<AccessLevel>> ShowAccessLevel(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) return Errors.User.NotFound;

        var permissions = await _permissionRepository.GetByRoleAsync(user.RoleId);

        if (permissions.Any(p => p.Name == "superAdmin"))
            return await GetFullAccess();

        var rootPath = GetRootPath(permissions);

        return new AccessLevel { Modules = MapPermissionsToModules(permissions), RootPath = rootPath };
    }

    private async Task<AccessLevel> GetFullAccess()
    {
        var permissions = await _permissionRepository.ListAsync();
        var modules = (await _moduleRepository.ListAllAsync()).Items.ToDictionary(m => m.Id);

        var groupedModules = permissions.Items
            .GroupBy(p => p.ModuleId)
            .Select(g => new ModuleAccess
            {
                Name = modules.TryGetValue(g.Key ?? Guid.Empty, out var module) ? module.Name : "General",
                Icon = module?.Icon ?? "pi pi-cog",
                Permissions = g.Select(p => new PermissionPolicy
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName
                }).Distinct().ToList(),
            })
            .ToList();

        groupedModules.Add(new ModuleAccess
        {
            Name = "Administracion",
            Icon = "pi pi-cog",
            Permissions =
            [
                new PermissionPolicy
                {
                    Name = "dashboard",
                    DisplayName = "Dashboard",
                },
                new PermissionPolicy
                {
                    Name = "logs",
                    DisplayName = "Logs",
                },
            ]
        });

        return new AccessLevel { Modules = groupedModules, RootPath = "/admin/dashboard" };
    }

    private string GetRootPath(IEnumerable<Permission> permissions)
    {
        return permissions.Any(p => p.Name == BachetonConstants.Permissions.SupervisorPermission) ? "/app/map" : "/app/report";
    }

    private List<ModuleAccess> MapPermissionsToModules(IEnumerable<Permission> permissions)
    {
        return permissions
            .GroupBy(p => p.Module)
            .Select(g => new ModuleAccess
            {
                Name = g.Key?.Name ?? "General",
                Icon = g.Key?.Icon ?? "pi pi-cog",
                Permissions = g.Select(p => new PermissionPolicy
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName
                }).Distinct().ToList(),
            })
            .ToList();
    }
}