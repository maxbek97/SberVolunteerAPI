using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;

namespace SberVolunteerAPI.Services
{
    public class OrganiserService
    {
        private readonly SberVolunteerContext _db;

        public OrganiserService(SberVolunteerContext db)
        {
            _db = db;
        }

        public async Task<List<OrganiserEventDTO>> GetOrganizerUpcomingEventsAsync(uint organizerId)
        {
            var now = DateTime.Now;

            return await _db.Events
                .Where(e =>
                    e.CreatorId == organizerId &&
                    e.DatetimeStart > now)
                .Select(e => new OrganiserEventDTO
                {
                    IdEvent = e.IdEvent,
                    EventTitle = e.EventTitle,
                    EventDescription = e.EventDescription,
                    DatetimeStart = e.DatetimeStart,
                    DatetimeEnd = e.DatetimeEnd,

                    ApprovedVolunteers = e.EventsToVolunteers
                        .Where(v => v.RequestStatus == "approved")
                        .Select(v => new VolunteerShortDTO
                        {
                            Id = v.IdVolunteerNavigation.IdUser,
                            FullName = v.IdVolunteerNavigation.UserSurname + " " +
                                       v.IdVolunteerNavigation.UserName + " " +
                                       v.IdVolunteerNavigation.UserMiddlename,
                            Hours = v.IdVolunteerNavigation.VolunteersHours,
                            RequestStatus = v.RequestStatus
                        }).ToList(),

                    PendingVolunteers = e.EventsToVolunteers
                        .Where(v => v.RequestStatus == "pending")
                        .Select(v => new VolunteerShortDTO
                        {
                            Id = v.IdVolunteerNavigation.IdUser,
                            FullName = v.IdVolunteerNavigation.UserSurname + " " +
                                       v.IdVolunteerNavigation.UserName + " " +
                                       v.IdVolunteerNavigation.UserMiddlename,
                            Hours = v.IdVolunteerNavigation.VolunteersHours,
                            RequestStatus = v.RequestStatus
                        }).ToList(),
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> CreateEventAsync(uint userId, CreationEventDTO dto)
        {
            var entity = new Event
            {
                EventTitle = dto.EventTitle,
                EventDescription = dto.EventDescription,
                DatetimeStart = dto.DatetimeStart,
                DatetimeEnd = dto.DatetimeEnd,
                CreationDate = DateTime.UtcNow,
                CreatorId = userId,
                EventState = "active"
            };

            _db.Events.Add(entity);
            await _db.SaveChangesAsync();

            return (true, "OK");
        }

        public async Task<bool> UpdateVolunteerRequestStatusAsync(uint organiserId, UserRequestStatusDTO requestStatus)
        {
            var eventEntity = await _db.Events
                .FirstOrDefaultAsync(e => e.IdEvent == requestStatus.EventId && e.CreatorId == organiserId);

            if (eventEntity == null)
                return false; // нет такого события, или не он его создал

            // 2 — ищем запись участника
            var record = await _db.EventsToVolunteers
                .FirstOrDefaultAsync(v => v.IdEvent == requestStatus.EventId && v.IdVolunteer == requestStatus.VolunteerId);

            if (record == null)
                return false;

            // 3 — обновляем статус
            record.RequestStatus = requestStatus.Status;
            await _db.SaveChangesAsync();

            return true;
        }


    }

}
