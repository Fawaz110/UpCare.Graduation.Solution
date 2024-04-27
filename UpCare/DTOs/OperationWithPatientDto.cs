using Core.Entities.UpCareEntities;

namespace UpCare.DTOs
{
    public class OperationWithPatientDto
    {
        public Operation Operation { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public DateTime DateTime { get; set; }
    }
}
