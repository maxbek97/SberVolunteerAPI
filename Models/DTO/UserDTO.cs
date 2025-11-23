namespace SberVolunteerAPI.Models.DTO
{
    public class UserDTO
    {
    public uint IdUser { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserSurname { get; set; } = null!;

    public string? UserMiddlename { get; set; }

    public uint? VolunteersHours { get; set; }
    }
}
