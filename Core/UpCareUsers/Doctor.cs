using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.UpCareUsers
{
    public class Doctor : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Speciality { get; set; }
        public bool IsSurgeon { get; set; } 
        public decimal ConsultationPrice { get; set; }
        public decimal AppointmentPrice { get; set; }
        public string? FK_AdminId { get; set; }
    }
}
