using FluentValidation;
using RunningEventsSystem.SharedKernel.Dto;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();

        RuleFor(x => x.Location)
            .NotEmpty();

        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.Now)
            .WithMessage("Event date must be in future");

        RuleFor(x => x.RegistrationDeadline)
            .LessThan(x => x.EventDate)
            .WithMessage("Deadline must be before event date");

        RuleFor(x => x.MaxParticipants)
            .GreaterThan(0);

        RuleFor(x => x.EntryFee)
            .GreaterThanOrEqualTo(0);
    }
}