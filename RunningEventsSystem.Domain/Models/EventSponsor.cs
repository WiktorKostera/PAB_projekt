using RunningEventsSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningEventsSystem.Domain.Models;

public class EventSponsor
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int SponsorId { get; set; }
    public Sponsor Sponsor { get; set; } = null!;

    public string ContributionType { get; set; } = string.Empty;

    public decimal? Value { get; set; }
}