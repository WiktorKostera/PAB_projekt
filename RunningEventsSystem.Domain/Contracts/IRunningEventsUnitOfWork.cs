using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Domain.Contracts;

public interface IRunningEventsUnitOfWork : IDisposable
{
    IEventRepository EventRepository { get; }
    IRegistrationRepository RegistrationRepository { get; }
    IUserRepository UserRepository { get; }
    ISponsorRepository SponsorRepository { get; }
    IResultRepository ResultRepository { get; }

    void Commit();
}