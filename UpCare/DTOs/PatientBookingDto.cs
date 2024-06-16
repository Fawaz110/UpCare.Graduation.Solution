using Core.UpCareEntities;

namespace UpCare.DTOs
{
    public class PatientBookingDto
    {
        public Core.UpCareUsers.Patient Patient { get; set; }
        public PatientBookRoom RoomInfo { get; set; }
    }
}
