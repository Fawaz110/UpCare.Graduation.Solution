using Core.Entities.UpCareEntities;

namespace UpCare.DTOs
{
    public class PatientRadiologyToReturnDto
    {
        public string ResultUrl { get; set; }
        public Radiology Radiology { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public DateTime DateTime { get; set; }
    }
}
