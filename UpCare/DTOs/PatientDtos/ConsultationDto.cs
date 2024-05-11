using System.ComponentModel.DataAnnotations;
using Core.UpCareEntities;

namespace UpCare.DTOs.PatientDtos
{
    public class ConsultationDto
    {
        [Required]
        public Core.UpCareUsers.Patient Patient{ get; set; }
        [Required]
        public Core.UpCareUsers.Doctor Doctor { get; set; }
        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        [Required]
        public ConsultationType Type { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
    }
}
