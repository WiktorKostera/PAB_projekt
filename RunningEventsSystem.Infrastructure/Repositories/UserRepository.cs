using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(RunningEventsDbContext context)
        : base(context)
    {
    }

    public User? GetByEmail(string email)
    {
        return ((RunningEventsDbContext)_context).Users
            .FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
    }
}
