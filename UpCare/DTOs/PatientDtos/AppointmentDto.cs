using Core.UpCareEntities;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs.PatientDtos
{
    public class AppointmentDto
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public AppointmentType Type { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public Core.UpCareUsers.Doctor Doctor { get; set; }
    }
}
