using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    private readonly RunningEventsDbContext _context;

    public EventRepository(RunningEventsDbContext context)
        : base(context)
    {
        _context = context;
    }

    public bool Exists(string name)
    {
        return _context.Events.Any(x => x.Name == name);
    }
}