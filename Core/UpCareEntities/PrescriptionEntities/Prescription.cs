using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities.PrescriptionEntities
{
    public class Prescription : BaseEntity
    {
        public string Diagnosis { get; set; }
        public string Details { get; set; }
        public string FK_DoctorId { get; set; }
        public string FK_PatientId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string Advice { get; set; }
        public string? MedicinePaymentIntentId { get; set; }
        public string? MedicineClientSecret { get; set; }
        public string? CheckupPaymentIntentId { get; set; }
        public string? CheckupClientSecret { get; set; }
        public string? RadiologyPaymentIntentId { get; set; }
        public string? RadiologyClientSecret { get; set; }
        public string? PrescriptionPaymentIntentId { get; set; }
        public string? PrescriptionClientSecret { get; set; }
    }
}
