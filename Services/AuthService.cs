using Microsoft.AspNetCore.Identity;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;


public class AuthService
{
    private readonly SberVolunteerContext _db;
    private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

    public AuthService(SberVolunteerContext db)
    {
        _db = db;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
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
}
