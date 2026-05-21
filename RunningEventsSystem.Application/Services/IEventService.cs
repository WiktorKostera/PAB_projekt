using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.Application.Services;

public interface IEventService
{
    List<EventDto> GetAll();
    EventDto GetById(int id);
    int Create(CreateEventDto dto);
    void Update(UpdateEventDto dto);
    void Delete(int id);
}