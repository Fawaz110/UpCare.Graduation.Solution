using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs.StaffDTOs.Pharmacy
{
    public class PharmacyRegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{1,6}$",
            ErrorMessage = "Password Must Have Minimum six characters, at least one uppercase letter, one lowercase letter, one number and one special character:")]
        public string Password { get; set; }
        [Required]
        public string AdminId { get; set; }
    }
}
