namespace SberVolunteerAPI.Models.DTO
{
    public class CreationEventDTO
    {

        public string EventTitle { get; set; } = null!;

        public string EventDescription { get; set; } = null!;

        public DateTime DatetimeStart { get; set; }

        public DateTime DatetimeEnd { get; set; }

    }
}
