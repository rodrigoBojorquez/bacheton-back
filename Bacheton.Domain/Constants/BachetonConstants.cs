namespace Bacheton.Domain.Constants;

public static class BachetonConstants
{
    public const string PermissionsClaim = "permissions";

    public static class Permissions
    {
        public const string SuperAccessPermission = "superAdmin";
        public const string SupervisorPermission = "monitoring:Reportes";
    }
    
    public static class Roles
    {
        public const string UserRole = "Usuario";
    }
}