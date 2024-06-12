using System.ComponentModel.DataAnnotations;

namespace Core.UpCareEntities
{
    public enum ConsultationType
    {
        OnlineChatConsultation, OnlineVideoConsultation, OfflineConsultation, OnlineEmergency, OfflineEmergency
    }
    public class PatientConsultation 
    {
        [Required]
        public string FK_DoctorId { get; set; }
        [Required]
        public string FK_PatientId { get; set; }
        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        [Required]
        public ConsultationType Type { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
