using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Application.User.Common;
using Bacheton.Domain.Constants;
using Bacheton.Domain.Entities;
using Bacheton.Domain.Errors;
using Bacheton.Infrastructure.Data;
using Bacheton.Infrastructure.Data.Entities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Bacheton.Infrastructure.Services.Auth;

public class JwtService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly BachetonDbContext _context;
    private readonly IPermissionRepository _permissionRepository;

    public JwtService(IRoleRepository roleRepository, IConfiguration config, BachetonDbContext context,
        IPermissionRepository permissionRepository)
    {
        _config = config;
        _context = context;
        _permissionRepository = permissionRepository;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var permissions = await _permissionRepository.GetByRoleAsync(user.RoleId);
        
        var claims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(BachetonConstants.PermissionsClaim, string.Join(",", permissions.Select(p => new FormattedPermission(p).ToString())))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Authentication:Key")!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Authentication:JwtExpireMinutes")),
            Issuer = _config.GetValue<string>("Authentication:Issuer")!,
            Audience = _config.GetValue<string>("Authentication:Audience")!,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        var existToken = await _context.RefreshTokens.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        return existToken is not null && existToken.Expires > DateTime.UtcNow;
    }

    public async Task StoreRefreshTokenAsync(string refreshToken, Guid userId)
    {
        var existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.UserId == userId);

        if (existingToken is not null)
        {
            // Actualizar el token existente
            existingToken.Token = refreshToken;
            existingToken.Expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Authentication:RefreshTokenExpireDays"));

            _context.RefreshTokens.Update(existingToken);
        }
        else
        {
            // Insertar un nuevo token si no existe
            var entity = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Authentication:RefreshTokenExpireDays"))
            };

            await _context.RefreshTokens.AddAsync(entity);
        }

        await _context.SaveChangesAsync();
    }


    public async Task<ErrorOr<AuthResult>> RefreshToken(string refreshToken)
    {
        var existToken = _context.RefreshTokens.Include(r => r.User)
            .FirstOrDefault(r => r.Token == refreshToken);

        if (existToken is null || existToken.Expires < DateTime.UtcNow)
            return Errors.User.InvalidRefreshToken;

        existToken.Token = GenerateRefreshToken();
        existToken.Expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Authentication:RefreshTokenExpireDays"));

        _context.RefreshTokens.Update(existToken);
        await _context.SaveChangesAsync();

        var token = await GenerateTokenAsync (existToken.User);

        return new AuthResult(token, existToken.Token);
    }

    public async Task DeleteRefreshTokenAsync(string refreshToken)
    {
        var existToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (existToken is not null)
        {
            _context.RefreshTokens.Remove(existToken);
            await _context.SaveChangesAsync();
        }
    }
}