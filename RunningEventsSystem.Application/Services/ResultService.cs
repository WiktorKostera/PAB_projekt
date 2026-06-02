using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Exceptions;
using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.SharedKernel.Dto;

public class ResultService : IResultService
{
    private readonly IRunningEventsUnitOfWork _uow;
    public ResultService(IRunningEventsUnitOfWork uow) { _uow = uow; }

    public List<ResultDto> GetByEvent(int eventId) =>
        _uow.ResultRepository.GetByEventWithDetails(eventId)
            .Select(r => new ResultDto
            {
                Id = r.Id,
                UserFullName = $"{r.User?.FirstName} {r.User?.LastName}",
                Place = r.Place,
                FinishTime = r.FinishTime
            }).ToList();

    public int Create(CreateResultDto dto)
    {
        var reg = _uow.RegistrationRepository.Get(dto.RegistrationId)
            ?? throw new NotFoundException("Registration not found");

        if (_uow.ResultRepository.Find(r => r.RegistrationId == dto.RegistrationId).Any())
            throw new BadRequestException("Result for this registration already exists");

        if (dto.FinishTime <= TimeSpan.Zero)
            throw new BadRequestException("Finish time must be greater than zero");

        var result = new Result
        {
            UserId = reg.UserId,
            EventId = reg.EventId,
            RegistrationId = dto.RegistrationId,
            FinishTime = dto.FinishTime,
            Place = dto.Place
        };
        _uow.ResultRepository.Insert(result);
        _uow.Commit();
        return result.Id;
    }
}
