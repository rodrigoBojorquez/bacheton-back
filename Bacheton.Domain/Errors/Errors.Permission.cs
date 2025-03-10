using ErrorOr;

namespace Bacheton.Domain.Errors;

public static partial class Errors
{
    public static class Permission
    {
        public static Error NotFound => Error.NotFound("Permission.NotFound", "Permiso no encontrado.");
    }
}