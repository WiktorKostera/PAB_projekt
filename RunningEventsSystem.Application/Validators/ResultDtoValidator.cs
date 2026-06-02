using FluentValidation;
using RunningEventsSystem.SharedKernel.Dto;

public class CreateResultDtoValidator : AbstractValidator<CreateResultDto>
{
    public CreateResultDtoValidator()
    {
        RuleFor(x => x.RegistrationId).GreaterThan(0);
        RuleFor(x => x.FinishTime).GreaterThan(TimeSpan.Zero);
        RuleFor(x => x.Place).GreaterThan(0);
    }
}
