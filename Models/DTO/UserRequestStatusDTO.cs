namespace SberVolunteerAPI.Models.DTO
{
    public class UserRequestStatusDTO
    {
        public uint EventId { get; set; }
        public uint VolunteerId { get; set; }
        public string Status { get; set; } = ""; // "approved" / "rejected"
    }
}
