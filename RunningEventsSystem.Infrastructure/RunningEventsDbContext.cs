using Microsoft.EntityFrameworkCore;
using RunningEventsSystem.Domain.Models;

namespace RunningEventsSystem.Infrastructure;

public class RunningEventsDbContext : DbContext
{
    public RunningEventsDbContext(DbContextOptions<RunningEventsDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<Sponsor> Sponsors => Set<Sponsor>();
    public DbSet<EventSponsor> EventSponsors => Set<EventSponsor>();
    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .Property(x => x.EntryFee)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Registration>()
            .HasIndex(x => new { x.UserId, x.EventId })
            .IsUnique(false);

        modelBuilder.Entity<Registration>()
            .Property(x => x.PaymentAmount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Registration>()
            .HasOne(x => x.User)
            .WithMany(x => x.Registrations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Registration>()
            .HasOne(x => x.Event)
            .WithMany(x => x.Registrations)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Result>()
            .HasIndex(x => x.RegistrationId)
            .IsUnique();

        modelBuilder.Entity<Result>()
            .HasOne(x => x.Registration)
            .WithOne(x => x.Result)
            .HasForeignKey<Result>(x => x.RegistrationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Result>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Result>()
            .HasOne(x => x.Event)
            .WithMany(x => x.Results)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
