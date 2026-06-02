using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Exceptions;
using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.SharedKernel.Dto;

public class ResultService : IResultService
{
    private readonly IRunningEventsUnitOfWork _uow;
    public ResultService(IRunningEventsUnitOfWork uow) { _uow = uow; }

    public List<ResultDto> GetAll() =>
        _uow.ResultRepository.GetAll()
            .Select(MapToDto)
            .OrderBy(r => r.EventId)
            .ThenBy(r => r.Place)
            .ToList();

    public ResultDto GetById(int id)
    {
        var result = _uow.ResultRepository.Get(id) ?? throw new NotFoundException("Result not found");
        return MapToDto(result);
    }

    public List<ResultDto> GetByEvent(int eventId) =>
        _uow.ResultRepository.GetByEventWithDetails(eventId)
            .Select(MapToDto).ToList();

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

    public void Update(UpdateResultDto dto)
    {
        var result = _uow.ResultRepository.Get(dto.Id) ?? throw new NotFoundException("Result not found");

        if (dto.FinishTime <= TimeSpan.Zero)
            throw new BadRequestException("Finish time must be greater than zero");

        if (dto.Place <= 0)
            throw new BadRequestException("Place must be greater than zero");

        result.FinishTime = dto.FinishTime;
        result.Place = dto.Place;
        _uow.Commit();
    }

    public void Delete(int id)
    {
        var result = _uow.ResultRepository.Get(id) ?? throw new NotFoundException("Result not found");
        _uow.ResultRepository.Delete(result);
        _uow.Commit();
    }

    private static ResultDto MapToDto(Result r) => new()
    {
        Id = r.Id,
        UserId = r.UserId,
        EventId = r.EventId,
        RegistrationId = r.RegistrationId,
        UserFullName = $"{r.User?.FirstName} {r.User?.LastName}".Trim(),
        Place = r.Place,
        FinishTime = r.FinishTime
    };
}
