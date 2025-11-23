namespace SberVolunteerAPI.Models.DTO
{
    public class EventDTO
    {
        public uint Id { get; set; }
        public string EventTitle { get; set; } = null!;

        public string EventDescription { get; set; } = null!;

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
