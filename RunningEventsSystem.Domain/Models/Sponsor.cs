using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningEventsSystem.Domain.Models;

public class Sponsor
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }

    public string? ContactEmail { get; set; }

    public List<EventSponsor> EventSponsors { get; set; } = new();
}