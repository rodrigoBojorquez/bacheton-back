using ErrorOr;

namespace Bacheton.Domain.Errors;

public static partial class Errors
{
    public static class Role
    {
        public static Error NotFound => Error.NotFound("Role.NotFound", "No se encontró el rol.");
    }
}