using ErrorOr;
using FluentValidation;
using MediatR;

namespace Bacheton.Application.Common.Behaviors;

/**
 * Pipeline de comportamiento para la validacion de los comandos y queries de MediatR
 *
 * - Se encarga de validar los comandos y queries antes de ser procesados
 * - Traslada los errores de validacion de FluentValidation a errores de dominio
 * - Esos errores de dominio pueden ser manejados por un middleware de errores
 */
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    
    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
            return await next();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
            return await next();

        // Convirtiendo los valores de FluentValidation en errores de dominio
        var errors = validationResult.Errors.ConvertAll(err => 
            Error.Validation(err.PropertyName, err.ErrorMessage));

        return (dynamic)errors;
    }
}