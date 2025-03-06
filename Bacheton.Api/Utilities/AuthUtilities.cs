using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Constants;

namespace Bacheton.Api.Utilities;

public class AuthUtilities : IAuthUtilities
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;

    public AuthUtilities(IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _httpContextAccessor = httpContextAccessor;
        _config = config;
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

        if (user is null) throw new AuthenticationException("Usuario no autenticado");

        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (claim is null) throw new AuthenticationException("Usuario no autenticado");

        return Guid.Parse(claim.Value);
    }

    public bool HasSuperAccess()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null) return false;
        
        var claim = user.Claims.FirstOrDefault(c => c.Type == BachetonConstants.PermissionsClaim);
        
        if (claim is null) return false;

        return claim.Value.Contains(BachetonConstants.SuperAccessPermission);
    }

    // TODO: implementar que de forma estructurada este metodo devuelva los accesos que tendran los usuarios, util para el front
    public object? ShowAccessLevel(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);

        var claim = token.Claims.FirstOrDefault(c => c.Type == BachetonConstants.PermissionsClaim);

        if (claim is null) return null;

        return claim.Value;
    }
}