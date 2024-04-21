using Core.UpCareEntities;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs
{
    public class EmergencyDto
    {
        [Required]
        public string PatientId { get; set; }
        [Required]
        public string Speciality { get; set; }
        [Required]
        [Range(3, 4, ErrorMessage = "The Type field must be either 3 or 4.")]
        public ConsultationType Type { get; set; }
    }
}
