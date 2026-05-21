using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningEventsSystem.Domain.Models;

public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

public class Registration
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public RegistrationStatus Status { get; set; }

    public int? StartNumber { get; set; }

    public Result? Result { get; set; }
}