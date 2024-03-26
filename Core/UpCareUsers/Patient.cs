using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.UpCareUsers
{
    public enum Gender
    {
        Male, Female
    }
    public class Patient : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? BloodType { get; set; }
        public Gender Gender { get; set; }
        public string? FK_ReceptionistId { get; set; }
    }
}
