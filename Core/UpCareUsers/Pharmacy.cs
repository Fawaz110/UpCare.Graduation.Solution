using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareUsers
{
    public class Pharmacy : IdentityUser
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
