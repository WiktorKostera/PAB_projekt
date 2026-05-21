using FluentValidation;
using RunningEventsSystem.SharedKernel.Dto;

public class UpdateEventDtoValidator : AbstractValidator<UpdateEventDto>
{
    public UpdateEventDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.EventDate).GreaterThan(DateTime.Now);
        RuleFor(x => x.MaxParticipants).GreaterThan(0);
        RuleFor(x => x.EntryFee).GreaterThanOrEqualTo(0);
    }
}