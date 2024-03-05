using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities
{
    public enum ConsultationType
    {
        OnlineChatConsultation, OnlineVideoConsultation, OfflineConsultation, OnlineEmergency, OfflineEmergency
    }
    public class PatientConsultation
    {
        public string FK_DoctorId { get; set; }
        public string FK_PatientId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public ConsultationType Type { get; set; }
    }
}
