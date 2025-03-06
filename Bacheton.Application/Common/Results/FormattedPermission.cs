using System.Text.RegularExpressions;
using Bacheton.Domain.Entities;

namespace Bacheton.Application.Common.Results;

public class FormattedPermission
{
    private static readonly Regex PermissionRegex = new(@"^([a-zA-Z]+):([a-zA-Z]+)$", RegexOptions.Compiled);

    public string? Module { get; }
    public string Permission { get; }

    public FormattedPermission(Permission permission)
    {
        if (permission is null)
        {
            throw new ArgumentNullException(nameof(permission), "The permission cannot be null.");
        }

        Module = permission.Module?.Name ?? null;
        Permission = permission.Name;
    }

    public override string ToString()
    {
        if (Module is null)
        {
            return Permission;
        }
        
        return $"{Permission}:{Module}";
    }

    public static bool TryParse(string input, out FormattedPermission? formattedPermission)
    {
        formattedPermission = null;

        var match = PermissionRegex.Match(input);
        if (!match.Success) return false;

        formattedPermission = new FormattedPermission(
            new Permission { Name = match.Groups[1].Value, Module = new Module { Name = match.Groups[2].Value } }
        );
        return true;
    }
}
