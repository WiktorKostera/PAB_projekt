using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
{
    private readonly RunningEventsDbContext _context;

    public RegistrationRepository(RunningEventsDbContext context)
        : base(context)
    {
        _context = context;
    }

    public bool IsUserRegistered(int userId, int eventId)
    {
        return _context.Registrations
            .Any(x => x.UserId == userId && x.EventId == eventId);
    }
}