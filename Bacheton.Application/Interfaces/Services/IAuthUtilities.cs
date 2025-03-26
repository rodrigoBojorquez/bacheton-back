using Bacheton.Application.Common.Results;
using ErrorOr;

namespace Bacheton.Application.Interfaces.Services;

public interface IAuthUtilities
{
    void SetRefreshToken(string token);
    Guid GetUserId();
    bool HasSuperAccess();
    Task<ErrorOr<AccessLevel>> ShowAccessLevel(Guid userId);
}