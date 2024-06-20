using Core.Entities.UpCareEntities;
using Core.UpCareEntities;
using Core.UpCareUsers;

namespace UpCare.DTOs.PrescriptionDtos
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; }
        public string Details { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string Advice { get; set; }
        public Doctor Doctor { get; set; }
        public string MedicinePaymentIntentId { get; set; }
        public string MedicineClientSecret { get; set; }
        public string CheckupPaymentIntentId { get; set; }
        public string CheckupClientSecret { get; set; }
        public string RadiologyPaymentIntentId { get; set; }
        public string RadiologyClientSecret { get; set; }
        public string PrescriptionPaymentIntentId { get; set; }
        public string PrescriptionClientSecret { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();
        public List<Checkup> Checkups { get; set; } = new List<Checkup>();
        public List<Radiology> Radiologies { get; set; } = new List<Radiology>();
    }
}
