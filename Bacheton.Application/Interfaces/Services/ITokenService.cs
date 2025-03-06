using Bacheton.Application.User.Common;
using ErrorOr;

namespace Bacheton.Application.Interfaces.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(Domain.Entities.User user);
    string GenerateRefreshToken();
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    Task StoreRefreshTokenAsync(string refreshToken, Guid userId);
    Task<ErrorOr<AuthResult>> RefreshToken(string refreshToken);
    Task DeleteRefreshTokenAsync(string refreshToken);
}