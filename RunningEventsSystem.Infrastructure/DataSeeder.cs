using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.Infrastructure;
using System.Security.Cryptography;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class DataSeeder
{
    private readonly RunningEventsDbContext _dbContext;

    public DataSeeder(RunningEventsDbContext context)
    {
        _dbContext = context;
    }

    public void Seed()
    {
        _dbContext.Database.EnsureCreated();
        ApplyDatabaseFixups();

        if (!_dbContext.Database.CanConnect()) return;

        // USERS
        if (!_dbContext.Users.Any())
        {
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan@test.pl", Password = HashPassword("test123"), Role = UserRole.Participant, IsActive = true },
                new User { Id = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna@test.pl", Password = HashPassword("test123"), Role = UserRole.Participant, IsActive = true },
                new User { Id = 3, FirstName = "Piotr", LastName = "Wiśniewski", Email = "piotr@test.pl", Password = HashPassword("test123"), Role = UserRole.Admin, IsActive = true },
                new User { Id = 4, FirstName = "Katarzyna", LastName = "Wójcik", Email = "kasia@test.pl", Password = HashPassword("test123"), Role = UserRole.Participant, IsActive = true },
                new User { Id = 5, FirstName = "Marek", LastName = "Zieliński", Email = "marek@test.pl", Password = HashPassword("test123"), Role = UserRole.Participant, IsActive = true }
            };
            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();
        }

        // EVENTS
        var seedEvents = new List<Event>
        {
            new Event
            {
                Id = 1,
                Name = "Cracovia Marathon 2025",
                Description = "Największy maraton w Krakowie",
                Location = "Rynek Główny",
                City = "Kraków",
                EventDate = DateTime.Now.AddMonths(2),
                RegistrationDeadline = DateTime.Now.AddMonths(1),
                MaxParticipants = 100,
                EntryFee = 80,
                IsActive = true,
                ImageUrl = "images/event-marathon.jpg"
            },
            new Event
            {
                Id = 2,
                Name = "Warsaw Night Run 2025",
                Description = "Nocny bieg ulicami Warszawy",
                Location = "Plac Defilad",
                City = "Warszawa",
                EventDate = DateTime.Now.AddMonths(3),
                RegistrationDeadline = DateTime.Now.AddMonths(2),
                MaxParticipants = 50,
                EntryFee = 60,
                IsActive = true,
                ImageUrl = "images/event-night-run.jpg"
            },
            new Event
            {
                Id = 3,
                Name = "Poznań Half Marathon 2024",
                Description = "Półmaraton przez centrum Poznania",
                Location = "Stary Rynek",
                City = "Poznań",
                EventDate = DateTime.Now.AddMonths(-2),
                RegistrationDeadline = DateTime.Now.AddMonths(-3),
                MaxParticipants = 200,
                EntryFee = 50,
                IsActive = false,
                ImageUrl = "images/event-half-marathon.jpg"
            },
            new Event
            {
                Id = 4,
                Name = "Gdańsk 10km 2024",
                Description = "Bieg przez Stare Miasto w Gdańsku",
                Location = "Długi Targ",
                City = "Gdańsk",
                EventDate = DateTime.Now.AddMonths(-1),
                RegistrationDeadline = DateTime.Now.AddMonths(-2),
                MaxParticipants = 150,
                EntryFee = 40,
                IsActive = false,
                ImageUrl = "images/event-10k.jpg"
            }
        };

        foreach (var seedEvent in seedEvents)
        {
            var existingEvent = _dbContext.Events.Find(seedEvent.Id);
            if (existingEvent == null)
            {
                _dbContext.Events.Add(seedEvent);
                continue;
            }

            existingEvent.Name = seedEvent.Name;
            existingEvent.Description = seedEvent.Description;
            existingEvent.Location = seedEvent.Location;
            existingEvent.City = seedEvent.City;
            existingEvent.EventDate = seedEvent.EventDate;
            existingEvent.RegistrationDeadline = seedEvent.RegistrationDeadline;
            existingEvent.MaxParticipants = seedEvent.MaxParticipants;
            existingEvent.EntryFee = seedEvent.EntryFee;
            existingEvent.IsActive = seedEvent.IsActive;

            if (string.IsNullOrWhiteSpace(existingEvent.ImageUrl))
            {
                existingEvent.ImageUrl = seedEvent.ImageUrl;
            }
        }
        _dbContext.SaveChanges();

        // REJESTRACJE dla przeszłych eventów
        if (!_dbContext.Registrations.Any())
        {
            var registrations = new List<Registration>
            {
                // Poznań - event 3
                new Registration { Id = 1, UserId = 1, EventId = 3, RegistrationDate = DateTime.Now.AddMonths(-3), Status = RegistrationStatus.Confirmed },
                new Registration { Id = 2, UserId = 2, EventId = 3, RegistrationDate = DateTime.Now.AddMonths(-3), Status = RegistrationStatus.Confirmed },
                new Registration { Id = 3, UserId = 4, EventId = 3, RegistrationDate = DateTime.Now.AddMonths(-3), Status = RegistrationStatus.Confirmed },
                // Gdańsk - event 4
                new Registration { Id = 4, UserId = 1, EventId = 4, RegistrationDate = DateTime.Now.AddMonths(-2), Status = RegistrationStatus.Confirmed },
                new Registration { Id = 5, UserId = 3, EventId = 4, RegistrationDate = DateTime.Now.AddMonths(-2), Status = RegistrationStatus.Confirmed },
                new Registration { Id = 6, UserId = 5, EventId = 4, RegistrationDate = DateTime.Now.AddMonths(-2), Status = RegistrationStatus.Confirmed },
            };
            _dbContext.Registrations.AddRange(registrations);
            _dbContext.SaveChanges();
        }

        // WYNIKI dla przeszłych eventów
        if (!_dbContext.Results.Any())
        {
            var results = new List<Result>
            {
                // Poznań wyniki
                new Result { Id = 1, UserId = 2, EventId = 3, RegistrationId = 2, Place = 1, FinishTime = new TimeSpan(1, 32, 14) },
                new Result { Id = 2, UserId = 4, EventId = 3, RegistrationId = 3, Place = 2, FinishTime = new TimeSpan(1, 38, 45) },
                new Result { Id = 3, UserId = 1, EventId = 3, RegistrationId = 1, Place = 3, FinishTime = new TimeSpan(1, 44, 02) },
                // Gdańsk wyniki
                new Result { Id = 4, UserId = 3, EventId = 4, RegistrationId = 5, Place = 1, FinishTime = new TimeSpan(0, 41, 33) },
                new Result { Id = 5, UserId = 1, EventId = 4, RegistrationId = 4, Place = 2, FinishTime = new TimeSpan(0, 43, 12) },
                new Result { Id = 6, UserId = 5, EventId = 4, RegistrationId = 6, Place = 3, FinishTime = new TimeSpan(0, 47, 58) },
            };
            _dbContext.Results.AddRange(results);
            _dbContext.SaveChanges();
        }
    }

    private static string HashPassword(string password)
    {
        const int saltSize = 16;
        const int keySize = 32;
        const int iterations = 100000;

        var salt = RandomNumberGenerator.GetBytes(saltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            keySize);

        return $"PBKDF2${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
    }

    private void ApplyDatabaseFixups()
    {
        AddColumnIfMissing("Registrations", "PaidAt", "ALTER TABLE Registrations ADD COLUMN PaidAt TEXT NULL");
        AddColumnIfMissing("Registrations", "PaymentAmount", "ALTER TABLE Registrations ADD COLUMN PaymentAmount TEXT NOT NULL DEFAULT '0.0'");
        AddColumnIfMissing("Registrations", "PaymentStatus", "ALTER TABLE Registrations ADD COLUMN PaymentStatus INTEGER NOT NULL DEFAULT 0");
        AddColumnIfMissing("Registrations", "CancelledAt", "ALTER TABLE Registrations ADD COLUMN CancelledAt TEXT NULL");
        AddColumnIfMissing("Registrations", "StartNumber", "ALTER TABLE Registrations ADD COLUMN StartNumber INTEGER NULL");
    }

    private void AddColumnIfMissing(string tableName, string columnName, string sql)
    {
        if (ColumnExists(tableName, columnName))
        {
            return;
        }

        _dbContext.Database.ExecuteSqlRaw(sql);
    }

    private bool ColumnExists(string tableName, string columnName)
    {
        var connection = _dbContext.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;

        if (shouldClose)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"PRAGMA table_info({tableName})";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        finally
        {
            if (shouldClose)
            {
                connection.Close();
            }
        }
    }

}
