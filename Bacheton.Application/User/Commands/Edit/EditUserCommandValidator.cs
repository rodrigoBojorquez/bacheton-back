using FluentValidation;

namespace Bacheton.Application.User.Commands.Edit;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
    .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
    .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.")
    .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una minúscula.")
    .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
    .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.")
    .When(x => !string.IsNullOrWhiteSpace(x.Password)); // Solo si hay contraseña enviada

    }
}