using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;

namespace SberVolunteerAPI.Services
{
    public class VolunteerService
    {
        private readonly SberVolunteerContext _db;

        public VolunteerService(SberVolunteerContext db)
        {
            _db = db;
        }

        public async Task<List<EventDTO>> GetAvailableEventsAsync(uint userId)
        {
            var now = DateTime.Now;

            return await _db.Events
                .Where(e =>
                    e.DatetimeStart > now && // событие в будущем
                    !e.EventsToVolunteers.Any(v => v.IdVolunteer == userId) // пользователь не записан
                )
                .Select(x => new EventDTO
                {
                    Id = x.IdEvent,
                    EventTitle = x.EventTitle,
                    EventDescription = x.EventDescription,
                    DatetimeStart = x.DatetimeStart,
                    DatetimeEnd = x.DatetimeEnd,
                    CreationDate = x.CreationDate
                })
                .ToListAsync();
        }
        public async Task<List<EventDTO>> GetMyEventsAsync(uint userId)
        {
            var now = DateTime.Now;

            return await _db.EventsToVolunteers
                .Where(v =>
                    v.IdVolunteer == userId &&
                    v.RequestStatus == "approved" &&
                    v.IdEventNavigation.DatetimeStart > now
        )
                .Select(x => new EventDTO
                {
                    Id = x.IdEventNavigation.IdEvent,
                    EventTitle = x.IdEventNavigation.EventTitle,
                    EventDescription = x.IdEventNavigation.EventDescription,
                    DatetimeStart = x.IdEventNavigation.DatetimeStart,
                    DatetimeEnd = x.IdEventNavigation.DatetimeEnd,
                    CreationDate = x.IdEventNavigation.CreationDate
                })
                .ToListAsync();
        }

        public async Task<List<EventDTO>> GetMyClosedEventsAsync(uint userId)
        {
            var now = DateTime.Now;

            return await _db.EventsToVolunteers
                .Where(v =>
                    v.IdVolunteer == userId &&
                    (v.VisitStatus == "came" || v.VisitStatus == "absent") &&
                    v.IdEventNavigation.EventState == "closed"
        )
                .Select(x => new EventDTO
                {
                    Id = x.IdEventNavigation.IdEvent,
                    EventTitle = x.IdEventNavigation.EventTitle,
                    EventDescription = x.IdEventNavigation.EventDescription,
                    DatetimeStart = x.IdEventNavigation.DatetimeStart,
                    DatetimeEnd = x.IdEventNavigation.DatetimeEnd,
                    CreationDate = x.IdEventNavigation.CreationDate
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> SubscribeToEventAsync(uint eventId, uint userId)
        {
            var ev = await _db.Events
                .FirstOrDefaultAsync(e => e.IdEvent == eventId);

            if (ev == null)
                return (false, "Event not found");

            var existing = await _db.EventsToVolunteers
                .FirstOrDefaultAsync(e => e.IdEvent == eventId && e.IdVolunteer == userId);

            if (existing != null)
                return (false, "You have already submitted a request for this event.");

            var record = new EventsToVolunteer
            {
                IdEvent = eventId,
                IdVolunteer = userId,
                RequestStatus = "pending",
                VisitStatus = "unknown"
            };

            _db.EventsToVolunteers.Add(record);
            await _db.SaveChangesAsync();

            return (true, "OK");
        }

    }

}
