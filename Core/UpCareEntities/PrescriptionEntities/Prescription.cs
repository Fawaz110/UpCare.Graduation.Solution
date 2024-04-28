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
    }
}
