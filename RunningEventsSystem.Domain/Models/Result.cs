using RunningEventsSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningEventsSystem.Domain.Models;

public class Result
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int RegistrationId { get; set; }
    public Registration Registration { get; set; } = null!;

    public TimeSpan FinishTime { get; set; }

    public int Place { get; set; }

    public TimeSpan? NetTime { get; set; }
    public TimeSpan? GrossTime { get; set; }
}