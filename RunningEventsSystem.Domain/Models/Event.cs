using RunningEventsSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningEventsSystem.Domain.Models;

public class Event
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }
    public DateTime RegistrationDeadline { get; set; }

    public int MaxParticipants { get; set; }

    public decimal EntryFee { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public List<Registration> Registrations { get; set; } = new();
    public List<EventSponsor> EventSponsors { get; set; } = new();
    public List<Result> Results { get; set; } = new();
}
