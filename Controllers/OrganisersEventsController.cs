using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;
using SberVolunteerAPI.Services;

namespace SberVolunteerAPI.Controllers
{
    [Authorize(Roles = "organiser")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisersEventsController : ControllerBase
    {
        private readonly OrganiserService _organiserService;

        public OrganisersEventsController(OrganiserService organiser_service)
        {
            _organiserService = organiser_service;
        }

        [HttpGet("future-events")]
        public async Task<IActionResult> GetUpcomingOrganiserEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint organizerId = uint.Parse(userIdString);

            var events = await _organiserService.GetOrganizerUpcomingEventsAsync(organizerId);
            return Ok(events);
        }

        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreationEventDTO req)
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint organizerId = uint.Parse(userIdString);

            var created = await _organiserService.CreateEventAsync(organizerId, req);
            return Ok(new { message = created.Message });
        }

        [HttpPost("update-volunteers-request")]
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
        [HttpGet("past-events")]
        public async Task<IActionResult> GetPastEvents()
        {
            var userIdString = User.FindFirst("userId")?.Value;
            if (userIdString == null)
                return Unauthorized("Invalid token");

            uint organizerId = uint.Parse(userIdString);

            var result = await _organiserService.GetPastEventsAsync(organizerId);

            return Ok(result);
        }

        [HttpPost("complete-event")]
        public async Task<IActionResult> CompleteEvent([FromBody] UserVisitStatusDTO dto)
        {
            uint organiserId = uint.Parse(User.FindFirst("userId")!.Value);

            var success = await _organiserService.CompleteEventAsync(organiserId, dto);

            if (!success)
                return BadRequest("Event not found or you are not the creator.");

            return Ok("Event completed successfully");
        }

    }
}
