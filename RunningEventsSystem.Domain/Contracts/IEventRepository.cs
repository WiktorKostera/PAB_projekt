using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Domain.Contracts;

public interface IEventRepository : IRepository<Event>
{
    bool Exists(string name);
}