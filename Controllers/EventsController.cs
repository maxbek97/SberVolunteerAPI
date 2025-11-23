using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;
using SberVolunteerAPI.Services;

namespace SberVolunteerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly VolunteerService _volunteerService;
        private readonly OrganiserService _organiserService;

        public EventsController(VolunteerService volunteer_service, OrganiserService organiser_service)
        {
            _volunteerService = volunteer_service;
            _organiserService = organiser_service;
        }

        [HttpGet("volunteer/futureEvents")]
        public async Task<IActionResult> GetAvailableEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _volunteerService.GetAvailableEventsAsync(userId);

            return Ok(events);
        }

        [HttpPost("volunteer/subscribe/{eventId}")]
        public async Task<IActionResult> SubscribeToEvent(uint eventId)
        {
            // Достаём userId из JWT
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            // Пытаемся создать запись подписки
            var result = await _volunteerService.SubscribeToEventAsync(eventId, userId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = "Subscription request created successfully" });
        }

        [HttpGet("volunteer/myEvents")]
        public async Task<IActionResult> GetMyEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _volunteerService.GetMyEventsAsync(userId);

            return Ok(events);
        }

        [HttpGet("volunteer/ClosedEvents")]
        public async Task<IActionResult> GetMyClosedEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _volunteerService.GetMyClosedEventsAsync(userId);

            return Ok(events);
        }

        [HttpGet("organiser/futureEvents")]
        public async Task<IActionResult> GetUpcomingOrganiserEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint organizerId = uint.Parse(userIdString);

            var events = await _organiserService.GetOrganizerUpcomingEventsAsync(organizerId);
            return Ok(events);
        }

        [HttpPost("organiser/create")]
        public async Task<IActionResult> CreateEvent([FromBody] CreationEventDTO req)
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint organizerId = uint.Parse(userIdString);

            var created = await _organiserService.CreateEventAsync(organizerId, req);
            return Ok(created);
        }

        [HttpPost("organiser/volunteers_request/update")]
        public async Task<IActionResult> UpdateStatus([FromBody] UserRequestStatusDTO dto)
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint organizerId = uint.Parse(userIdString);

            if (dto.Status != "approved" && dto.Status != "rejected")
                return BadRequest("Invalid status");

            var ok = await _organiserService.UpdateVolunteerRequestStatusAsync(organizerId, dto);
            if (!ok)
                return NotFound("Record not found");

            return Ok("Status updated");
        }

    }
}
