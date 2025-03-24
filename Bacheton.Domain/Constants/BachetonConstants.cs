namespace Bacheton.Domain.Constants;

public static class BachetonConstants
{
    public const string PermissionsClaim = "permissions";

    public static class Permissions
    {
        public const string SuperAccessPermission = "superAdmin:Administracion";
        public const string SupervisorPermission = "monitoring";
    }
    
    public static class Roles
    {
        public const string UserRole = "Usuario";
    }
}