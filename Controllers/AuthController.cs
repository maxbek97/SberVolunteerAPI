using Microsoft.AspNetCore.Mvc;
using SberVolunteerAPI.Models.DTO;
using SberVolunteerAPI.Services;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var (success, message) = await _authService.RegisterAsync(dto);

        if (!success)
            return BadRequest(new { message });

        return Ok(new { message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.Success)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Unauthorized"
            });
        }

        return Ok(new
        {
            success = true,
            token = result.Token,
            message = result.UserRole
        });

    }

    [HttpGet("user-info")]
    public async Task<IActionResult> GetVolunteerInfo()
    {
        var userIdString = User.FindFirst("userId")?.Value;
        if (userIdString == null)
            return Unauthorized("Invalid token");

        uint userId = uint.Parse(userIdString);

        var info = await _authService.GetVolunteerInfoAsync(userId);

        if (info == null)
            return NotFound("User not found.");

        return Ok(info);
    }

}
