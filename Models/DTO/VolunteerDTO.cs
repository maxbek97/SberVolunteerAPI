namespace SberVolunteerAPI.Models.DTO
{
    public class VolunteerShortDTO
    {
        public uint Id { get; set; }
        public string FullName { get; set; } = "";
        public uint? Hours { get; set; }
        public string RequestStatus { get; set; } = "";
    }
}
