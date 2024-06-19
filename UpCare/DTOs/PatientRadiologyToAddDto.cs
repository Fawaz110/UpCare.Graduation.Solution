using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs
{
    public class PatientRadiologyToAddDto
    {
        [Required]
        public string PatientId { get; set; }
        [Required]
        public int RadiologyId { get; set; }
        [Required]
        public IFormFile Result { get; set; }
    }
}
