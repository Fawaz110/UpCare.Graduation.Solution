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
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();
        public List<Checkup> Checkups { get; set; } = new List<Checkup>();
        public List<Radiology> Radiologies { get; set; } = new List<Radiology>();
    }
}
