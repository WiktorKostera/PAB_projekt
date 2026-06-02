using RunningEventsSystem.SharedKernel.Dto;
namespace RunningEventsSystem.Application.Services;

public interface IResultService
{
    List<ResultDto> GetAll();
    ResultDto GetById(int id);
    List<ResultDto> GetByEvent(int eventId);
    int Create(CreateResultDto dto);
    void Update(UpdateResultDto dto);
    void Delete(int id);
}
