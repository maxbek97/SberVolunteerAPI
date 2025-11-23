namespace SberVolunteerAPI.Models.DTO
{
    public class OrganiserEventDTO
    {
        public uint IdEvent { get; set; }
        public string EventTitle { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeEnd { get; set; }

        public List<VolunteerShortDTO> ApprovedVolunteers { get; set; } = new();
        public List<VolunteerShortDTO> PendingVolunteers { get; set; } = new();
    }
}
