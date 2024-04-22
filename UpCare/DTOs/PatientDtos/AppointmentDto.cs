using Core.UpCareEntities;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs.PatientDtos
{
    public class AppointmentDto
    {
        [Required]
        public Core.UpCareUsers.Patient Patient { get; set; }
        [Required]
        public Core.UpCareUsers.Doctor Doctor { get; set; }
        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        [Required]
        public AppointmentType Type { get; set; }
    }
}
