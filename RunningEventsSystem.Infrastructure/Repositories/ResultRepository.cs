using Microsoft.EntityFrameworkCore;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class ResultRepository : Repository<Result>, IResultRepository
{
    public ResultRepository(RunningEventsDbContext context)
        : base(context)
    {
    }

    public IList<Result> GetByEventWithDetails(int eventId)
    {
        return ((RunningEventsDbContext)_context).Results
            .Include(x => x.User)
            .AsNoTracking()
            .Where(x => x.EventId == eventId)
            .OrderBy(x => x.Place)
            .ToList();
    }
}
