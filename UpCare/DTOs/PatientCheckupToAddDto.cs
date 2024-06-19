using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs
{
    public class PatientCheckupToAddDto
    {
        [Required]
        public string PatientId { get; set; }
        [Required]
        public int CheckupId { get; set; }
        [Required]
        public IFormFile Result { get; set; }
    }
}
