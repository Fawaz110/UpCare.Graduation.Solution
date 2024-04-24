using System.ComponentModel.DataAnnotations;

namespace Core.UpCareEntities
{
    public class Feedback : BaseEntity
    {
        public string FK_PatientId { get; set; }
        [Range(0, 5, ErrorMessage = "The Rate must be between 0 and 5.")]
        public decimal Rate { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
