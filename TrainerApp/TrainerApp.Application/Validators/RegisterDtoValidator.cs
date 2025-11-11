using FluentValidation;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Validators;

public class RegisterDtoValidator: AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).MinimumLength(6);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Role).Must(r => r is "Trainer" or "Client")
            .WithMessage("Role must be either Trainer or Client.");
    }
}