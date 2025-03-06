namespace Bacheton.Application.User.Common;

public record UserResult(Guid Id, string Name, string? Email, string RoleName, Guid RoleId);