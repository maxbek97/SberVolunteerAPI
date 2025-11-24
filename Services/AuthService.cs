using Microsoft.AspNetCore.Identity;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using SberVolunteerAPI.Services;


public class AuthService
{
    private readonly SberVolunteerContext _db;
    private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();
    private JWTService _jwtService;

    public AuthService(SberVolunteerContext db, JWTService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO dto)
    {
        // проверяем уникальность логина
        if (await _db.Users.AnyAsync(u => u.UserLogin == dto.UserLogin))
            return (false, "Login already exists");

        // создаём пользователя
        var user = new User
        {
            UserLogin = dto.UserLogin,
            UserRole = dto.UserRole,
            UserName = dto.UserName,
            UserSurname = dto.UserSurname,
            UserMiddlename = dto.UserMiddlename,
            VolunteersHours = 0
        };

        // хешируем пароль
        user.Password = _hasher.HashPassword(user, dto.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return (true, "Registration successful");
    }
    public async Task<(bool Success, string Token, string UserRole)> LoginAsync(LoginDTO dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserLogin == dto.UserLogin);
        if (user == null)
            return (false, "", "User not found");

        var result = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Success)
        {
            var token = _jwtService.GenerateToken(user);
            return (true, token, user.UserRole.ToString());
        }
        else
        {
            return (false, "", "Incorrect password");
        }
            
    }
    public async Task<UserDTO> GetVolunteerInfoAsync(uint userId)
    {
        return await _db.Users
            .Where(u => u.IdUser == userId)
            .Select(u => new UserDTO
            {
                IdUser = u.IdUser,
                UserLogin = u.UserLogin,
                UserRole = u.UserRole,
                UserName = u.UserName,
                UserSurname = u.UserSurname,
                UserMiddlename = u.UserMiddlename,
                VolunteersHours = u.VolunteersHours
            })
            .FirstOrDefaultAsync();
    }

}
