using System.ComponentModel.DataAnnotations;
using Core.UpCareEntities;

namespace UpCare.DTOs.PatientDtos
{
    public class ConsultationDto
    {
        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        [Required]
        public ConsultationType Type { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public Core.UpCareUsers.Patient Patient{ get; set; }
        public Core.UpCareUsers.Doctor Doctor { get; set; }
    }
}
