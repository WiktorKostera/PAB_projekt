using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly RunningEventsDbContext _context;

    public UserRepository(RunningEventsDbContext context)
        : base(context)
    {
        _context = context;
    }

    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(x => x.Email == email);
    }
}