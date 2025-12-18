namespace SberVolunteerAPI.Models.DTO
{
    public class ClosedEventDTO
    {
        public uint Id { get; set; }
        public string EventTitle { get; set; } = null!;
        public string EventDescription { get; set; } = null!;

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

        public DateTime CreationDate { get; set; }

        public string attendance { get; set; }
        public int duracity { get; set; }
    }
}
