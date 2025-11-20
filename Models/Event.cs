using System;
using System.Collections.Generic;

namespace SberVolunteerAPI.Models;

public partial class Event
{
    public uint IdEvent { get; set; }

    public string EventTitle { get; set; } = null!;

    public string EventDescription { get; set; } = null!;

    public DateTime DatetimeStart { get; set; }

    public DateTime DatetimeEnd { get; set; }

    public DateTime CreationDate { get; set; }

    public uint CreatorId { get; set; }

    public string EventState { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<EventsToVolunteer> EventsToVolunteers { get; set; } = new List<EventsToVolunteer>();
}
