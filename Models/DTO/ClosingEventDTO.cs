namespace SberVolunteerAPI.Models.DTO
{
    public class ClosingEventDTO
    {
        public uint IdEvent { get; set; }
        public string EventTitle { get; set; } = null!;
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeEnd { get; set; }

        public List<ParticipantDTO> Participants { get; set; } = new();
    }
}
