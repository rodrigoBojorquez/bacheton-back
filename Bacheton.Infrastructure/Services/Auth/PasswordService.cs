using Bacheton.Application.Interfaces.Services;

namespace Bacheton.Infrastructure.Services.Auth;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12, enhancedEntropy: false);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash, enhancedEntropy: false);
    }

    public string GenerateRandomPassword()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }
}