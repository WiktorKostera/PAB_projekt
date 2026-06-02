using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Domain.Contracts;

public interface IRegistrationRepository : IRepository<Registration>
{
    bool IsUserRegistered(int userId, int eventId);
    IList<Registration> GetByEventWithDetails(int eventId);
    IList<Registration> GetByUserWithDetails(int userId);
    Registration? GetActiveByUserAndEvent(int userId, int eventId);
}
