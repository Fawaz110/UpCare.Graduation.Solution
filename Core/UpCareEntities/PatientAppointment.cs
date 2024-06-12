using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities
{
    public enum AppointmentType
    {
        Offline, Online
    }
    public class PatientAppointment
    {
        public string FK_PatientId { get; set; }
        public string FK_DoctorId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public AppointmentType Type { get; set; }
    }
}
