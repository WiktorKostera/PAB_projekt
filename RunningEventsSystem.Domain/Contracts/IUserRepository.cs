using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Domain.Contracts;

public interface IUserRepository : IRepository<User>
{
    User? GetByEmail(string email);
}