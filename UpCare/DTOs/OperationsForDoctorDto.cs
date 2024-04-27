using Core.UpCareUsers;

namespace UpCare.DTOs
{
    public class OperationsForDoctorDto
    {
        public Doctor Doctor { get; set; }
        public List<OperationWithPatientDto> Data { get; set; }
    }
}
