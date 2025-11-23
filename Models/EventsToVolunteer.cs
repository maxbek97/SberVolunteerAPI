using System;
using System.Collections.Generic;

namespace SberVolunteerAPI.Models;

public partial class EventsToVolunteer
{
    public uint IdRecord { get; set; }

    public uint IdEvent { get; set; }

    public uint IdVolunteer { get; set; }

    public string? RequestStatus { get; set; }

    public string? VisitStatus { get; set; }

    public virtual Event IdEventNavigation { get; set; } = null!;

    public virtual User IdVolunteerNavigation { get; set; } = null!;
}
