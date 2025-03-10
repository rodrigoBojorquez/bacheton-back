namespace Bacheton.Application.Permissions.Common;

public record PermissionResult(
    Guid Id,
    string Name,
    string DisplayName,
    string? Icon,
    Guid? ModuleId,
    string? ModuleName);