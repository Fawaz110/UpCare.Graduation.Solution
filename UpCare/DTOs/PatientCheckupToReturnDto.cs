using Core.Entities.UpCareEntities;

namespace UpCare.DTOs
{
    public class PatientCheckupToReturnDto
    {
        public string ResultUrl { get; set; }
        public Checkup Checkup { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public DateTime DateTime { get; set; }
    }
}
