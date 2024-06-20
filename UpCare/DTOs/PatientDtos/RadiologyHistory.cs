using Core.Entities.UpCareEntities;

namespace UpCare.DTOs.PatientDtos
{
    public class RadiologyHistory
    {
        public Radiology Radiology { get; set; }
        public DateTime DateTime { get; set; }
        public string Result { get; set; }
    }
}
