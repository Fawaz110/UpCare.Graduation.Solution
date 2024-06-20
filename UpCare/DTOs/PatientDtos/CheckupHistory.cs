using Core.Entities.UpCareEntities;

namespace UpCare.DTOs.PatientDtos
{
    public class CheckupHistory
    {
        public Checkup Checkup { get; set; }
        public DateTime DateTime { get; set; }
        public string Result { get; set; }
    }
}
