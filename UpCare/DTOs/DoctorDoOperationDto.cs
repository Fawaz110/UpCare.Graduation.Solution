using Core.Entities.UpCareEntities;
using Core.UpCareUsers;

namespace UpCare.DTOs
{
    public class DoctorDoOperationDto
    {
        public Operation Operation { get; set; }
        public Doctor Doctor { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public Admin Admin { get; set; }
        public DateTime Date { get; set; }
    }
}
