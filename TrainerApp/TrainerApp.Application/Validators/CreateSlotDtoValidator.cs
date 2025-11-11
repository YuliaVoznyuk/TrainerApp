using FluentValidation;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Validators;

public class CreateSlotDtoValidator : AbstractValidator<CreateSlotDto>
{
    public CreateSlotDtoValidator()
    {
        RuleFor(x => x.EndAt)
            .GreaterThan(x => x.StartAt)
            .WithMessage("End time must be after start time.");

        RuleFor(x => x.MaxClients)
            .InclusiveBetween(1, 3)
            .WithMessage("MaxClients must be between 1 and 3.");
    }
}