using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs
{
    public class MedicineDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Indecations { get; set; }
        [Required]
        public string SideEffects { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string FK_PharmacyId { get; set; }
    }
}
