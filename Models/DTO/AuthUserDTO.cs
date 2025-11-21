namespace SberVolunteerAPI.Models.DTO
{
    public class RegisterDTO
    {
        public string UserLogin { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserSurname { get; set; } = null!;
        public string? UserMiddlename { get; set; }
    }
    public class LoginDTO
    {
        public string UserLogin { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
