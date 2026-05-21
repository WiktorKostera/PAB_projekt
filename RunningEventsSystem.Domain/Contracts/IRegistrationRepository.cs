using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Domain.Contracts;

public interface IRegistrationRepository : IRepository<Registration>
{
    bool IsUserRegistered(int userId, int eventId);
}