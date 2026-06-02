using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Domain.Contracts;

public interface IResultRepository : IRepository<Result>
{
    IList<Result> GetByEventWithDetails(int eventId);
}
