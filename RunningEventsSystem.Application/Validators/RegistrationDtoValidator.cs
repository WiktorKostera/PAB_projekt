using FluentValidation;
using RunningEventsSystem.SharedKernel.Dto;

public class CreateRegistrationDtoValidator : AbstractValidator<CreateRegistrationDto>
{
    public CreateRegistrationDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.EventId).GreaterThan(0);
    }
}
