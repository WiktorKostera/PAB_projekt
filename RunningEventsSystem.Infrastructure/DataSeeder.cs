using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.Infrastructure;

public class DataSeeder
{
    private readonly RunningEventsDbContext _dbContext;

    public DataSeeder(RunningEventsDbContext context)
    {
        _dbContext = context;
    }

    public void Seed()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Database.CanConnect())
        {
            if (!_dbContext.Events.Any())
            {
                var events = new List<Event>
                {
                    new Event
                    {
                        Id = 1,
                        Name = "Cracovia Marathon",
                        City = "Kraków",
                        EventDate = DateTime.Now.AddMonths(1)
                    },
                    new Event
                    {
                        Id = 2,
                        Name = "Warsaw Night Run",
                        City = "Warszawa",
                        EventDate = DateTime.Now.AddMonths(2)
                    }
                };

                _dbContext.Events.AddRange(events);
                _dbContext.SaveChanges();
            }
        }
    }
}