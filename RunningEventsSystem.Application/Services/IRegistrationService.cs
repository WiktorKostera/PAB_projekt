using RunningEventsSystem.SharedKernel.Dto;
namespace RunningEventsSystem.Application.Services;

public interface IRegistrationService
{
    List<RegistrationDto> GetAll();
    RegistrationDto GetById(int id);
    List<RegistrationDto> GetByEvent(int eventId);
    List<RegistrationDto> GetByUser(int userId);
    int Create(CreateRegistrationDto dto);
    void Cancel(int id);
    void CancelByUserAndEvent(int userId, int eventId);
    void Pay(int id);
    void Update(UpdateRegistrationDto dto);
}
