using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SberVolunteerAPI.Services;

namespace SberVolunteerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _service;

        public EventsController(EventService service)
        {
            _service = service;
        }

        [HttpGet("futureEvents")]
        public async Task<IActionResult> GetAvailableEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            var events = await _service.GetAvailableEventsAsync(userId);

            return Ok(events);
        }

        [HttpPost("subscribe/{eventId}")]
        public async Task<IActionResult> SubscribeToEvent(uint eventId)
        {
            // Достаём userId из JWT
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint userId = uint.Parse(userIdString);

            // Пытаемся создать запись подписки
            var result = await _service.SubscribeToEventAsync(eventId, userId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = "Subscription request created successfully" });
        }

    }
}
