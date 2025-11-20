using System;
using System.Collections.Generic;

namespace SberVolunteerAPI.Models;

public partial class User
{
    public uint IdUser { get; set; }

    public string UserLogin { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserSurname { get; set; } = null!;

    public string? UserMiddlename { get; set; }

    public uint? VolunteersHours { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<EventsToVolunteer> EventsToVolunteers { get; set; } = new List<EventsToVolunteer>();
}
