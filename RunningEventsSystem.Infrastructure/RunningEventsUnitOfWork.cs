using RunningEventsSystem.Domain.Contracts;

namespace RunningEventsSystem.Infrastructure;

public class RunningEventsUnitOfWork : IRunningEventsUnitOfWork
{
    private readonly RunningEventsDbContext _context;

    public IEventRepository EventRepository { get; }
    public IRegistrationRepository RegistrationRepository { get; }
    public IUserRepository UserRepository { get; }
    public ISponsorRepository SponsorRepository { get; }
    public IResultRepository ResultRepository { get; }

    public RunningEventsUnitOfWork(
        RunningEventsDbContext context,
        IEventRepository eventRepository,
        IRegistrationRepository registrationRepository,
        IUserRepository userRepository,
        ISponsorRepository sponsorRepository,
        IResultRepository resultRepository)
    {
        _context = context;

        EventRepository = eventRepository;
        RegistrationRepository = registrationRepository;
        UserRepository = userRepository;
        SponsorRepository = sponsorRepository;
        ResultRepository = resultRepository;
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}