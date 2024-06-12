using Core.Entities.UpCareEntities;
using Core.UpCareUsers;

namespace UpCare.DTOs
{
    public class NurseCareDto
    {
        public string? Suger { get; set; }
        public string? BeatPerMinute { get; set; }
        public string? OxygenSaturation { get; set; }
        public string? BloodPresure { get; set; }
        public DateTime DateTime { get; set; } 
        public string Notes { get; set; }
        public Nurse Nurse { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public Room Room { get; set; }
    }
}
