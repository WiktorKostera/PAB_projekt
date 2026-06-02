using Microsoft.EntityFrameworkCore;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
{
    public RegistrationRepository(RunningEventsDbContext context)
        : base(context)
    {
    }

    public bool IsUserRegistered(int userId, int eventId)
    {
        return ((RunningEventsDbContext)_context).Registrations
            .Any(x => x.UserId == userId && x.EventId == eventId && x.Status != RegistrationStatus.Cancelled);
    }

    public IList<Registration> GetByEventWithDetails(int eventId)
    {
        return ((RunningEventsDbContext)_context).Registrations
            .Include(x => x.User)
            .Include(x => x.Event)
            .AsNoTracking()
            .Where(x => x.EventId == eventId)
            .ToList();
    }

    public IList<Registration> GetByUserWithDetails(int userId)
    {
        return ((RunningEventsDbContext)_context).Registrations
            .Include(x => x.User)
            .Include(x => x.Event)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.RegistrationDate)
            .ToList();
    }

    public Registration? GetActiveByUserAndEvent(int userId, int eventId)
    {
        return ((RunningEventsDbContext)_context).Registrations
            .FirstOrDefault(x => x.UserId == userId && x.EventId == eventId && x.Status != RegistrationStatus.Cancelled);
    }
}
