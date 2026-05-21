using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class ResultRepository : Repository<Result>, IResultRepository
{
    public ResultRepository(RunningEventsDbContext context)
        : base(context)
    {
    }
}