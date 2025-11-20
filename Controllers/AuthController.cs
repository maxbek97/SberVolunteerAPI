using Microsoft.AspNetCore.Mvc;
using SberVolunteerAPI.Models.DTO;

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
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var (success, message) = await _authService.RegisterAsync(dto);

        if (!success)
            return BadRequest(new { message });

        return Ok(new { message });
    }
}
