using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;
using SberVolunteerAPI.Services;

namespace SberVolunteerAPI.Controllers
{
    [Authorize(Roles = "volunteer")]
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersEventsController : ControllerBase
    {
        private readonly VolunteerService _volunteerService;

        public VolunteersEventsController(VolunteerService volunteer_service)
        {
            _volunteerService = volunteer_service;
        }

        [HttpGet("future-events")]
        public async Task<IActionResult> GetAvailableEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _volunteerService.GetAvailableEventsAsync(userId);

            return Ok(events);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToEvent([FromQuery] uint eventId)
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

        [HttpGet("my-events")]
        public async Task<IActionResult> GetMyEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _volunteerService.GetMyEventsAsync(userId);

            return Ok(events);
        }

        [HttpGet("closed-events")]
        public async Task<IActionResult> GetMyClosedEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _volunteerService.GetMyClosedEventsAsync(userId);

            return Ok(events);
        }
    }
}
