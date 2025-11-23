namespace SberVolunteerAPI.Models.DTO
{
    public class UserVisitStatusDTO
    {
        public uint EventId { get; set; }
        public Dictionary<uint, bool> Attendance { get; set; } = new();
    }
}
