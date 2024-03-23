using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.UpCareUsers
{
    public class CheckupLab : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string? FK_AdminId { get; set; }
    }
}
