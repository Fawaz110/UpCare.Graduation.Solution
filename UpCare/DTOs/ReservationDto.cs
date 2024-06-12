using Core.UpCareUsers;

namespace UpCare.DTOs
{
    public class ReservationDto
    {
        public Core.UpCareUsers.Patient Patient { get; set; }
        public DateTime DateTime { get; set; }
        public bool Passed { get; set; }
        public string Type { get; set; }
    }
}
