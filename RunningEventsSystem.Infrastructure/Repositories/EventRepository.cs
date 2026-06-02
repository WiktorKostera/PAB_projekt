using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(RunningEventsDbContext context)
        : base(context)
    {
    }

    public bool Exists(string name)
    {
        return ((RunningEventsDbContext)_context).Events.Any(x => x.Name == name);
    }
}
