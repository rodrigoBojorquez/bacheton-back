using ErrorOr;

namespace Bacheton.Domain.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => 
            Error.NotFound("User.NotFound", "No se encontró el usuario.");
        
        public static Error InvalidCredentials => 
            Error.Unauthorized("User.InvalidCredentials", "Credenciales inválidas.");
        
        public static Error EmailAlreadyExists => 
            Error.Conflict("User.EmailAlreadyExists", "El correo electrónico ya está registrado.");
        
        public static Error WrongAuthenticationMethod => 
            Error.Conflict("User.WrongAuthenticationMethod", "Inicie sesión con Google.");
        
        public static Error InvalidRefreshToken => 
            Error.Unauthorized("User.InvalidRefreshToken", "Refresh Token inválido.");
        
        public static Error InvalidToken =>
            Error.Unauthorized("User.InvalidToken", "Token inválido.");
        
        public static Error MissingRefreshToken =>
            Error.Unauthorized("User.MissingRefreshToken", "Refresh Token no encontrado.");
        
        public static Error Unauthorized =>
            Error.Unauthorized("User.Unauthorized", "No autorizado.");
    }
}