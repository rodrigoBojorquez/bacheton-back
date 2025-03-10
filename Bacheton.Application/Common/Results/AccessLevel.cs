namespace Bacheton.Application.Common.Results;

public class AccessLevel
{
    public List<ModuleAccess> Modules { get; set; } = [];
}

public class ModuleAccess
{
    public required string Name { get; set; }
    public string? Icon { get; set; }
    public List<string> Permissions { get; set; } = [];
}
