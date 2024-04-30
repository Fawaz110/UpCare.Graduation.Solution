using Core.UpCareEntities;

namespace UpCare.DTOs.PatientDtos
{
    public class MedicineHistory
    {
        public Medicine Medicine { get; set; }
        public DateTime DateTime { get; set; }
    }
}
