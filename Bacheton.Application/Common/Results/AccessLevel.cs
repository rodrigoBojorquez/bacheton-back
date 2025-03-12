namespace Bacheton.Application.Common.Results;

public class AccessLevel
{
    public List<ModuleAccess> Modules { get; set; } = [];
    public string RootPath { get; set; }
}

public class ModuleAccess
{
    public required string Name { get; set; }
    public string? Icon { get; set; }
    public List<PermissionPolicy> Permissions { get; set; } = [];
}

public class PermissionPolicy
{
    public required string Name { get; set; }
    public string? ClientPath { get; set; }
}