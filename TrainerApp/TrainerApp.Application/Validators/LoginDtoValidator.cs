using FluentValidation;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Validators;


public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}