using RunningEventsSystem.SharedKernel.Dto;
namespace RunningEventsSystem.Application.Services;

public interface IResultService
{
    List<ResultDto> GetByEvent(int eventId);
    int Create(CreateResultDto dto);
}