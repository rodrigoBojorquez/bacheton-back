namespace Bacheton.Application.Roles.Common;

public record RoleResult(Guid Id, string Name,  string? Description, List<Guid> Permissions);