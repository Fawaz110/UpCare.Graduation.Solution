using Core.UpCareUsers;
using UpCare.DTOs.PatientDtos;

namespace UpCare.DTOs
{
    public class ScheduleDto
    {
        public Doctor Doctor { get; set; }
        public List<ReservationDto> Conversations { get; set; } = new List<ReservationDto>();
    }
}
