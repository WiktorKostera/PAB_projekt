using Microsoft.EntityFrameworkCore;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure;

public class RunningEventsDbContext : DbContext
{
    public RunningEventsDbContext(DbContextOptions<RunningEventsDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<EventSponsor> EventSponsors { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}