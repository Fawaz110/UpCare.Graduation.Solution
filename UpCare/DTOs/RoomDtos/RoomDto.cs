using Core.UpCareUsers;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs.RoomDtos
{
    public class RoomDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal PricePerNight { set; get; }
        [Required]
        public int NumberOfBeds { set; get; }
        [Required]
        public int AvailableBedsNumber { set; get; }
        [Required]
        public Receptionist Receptionist { set; get; }
        public List<PatientBookingDto> PatientBooking { get; set; }
    }
}
