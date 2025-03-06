namespace Bacheton.Application.Interfaces.Services;

public interface IAuthUtilities
{
    void SetRefreshToken(string token);
    Guid GetUserId();
    bool HasSuperAccess();
}