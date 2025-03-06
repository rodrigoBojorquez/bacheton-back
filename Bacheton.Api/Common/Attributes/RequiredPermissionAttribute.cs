namespace Bacheton.Api.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class RequiredPermissionAttribute(string permission) : Attribute
{
    public readonly string Permission = permission;
}