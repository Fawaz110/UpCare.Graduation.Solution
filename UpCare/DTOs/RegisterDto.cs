﻿using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayFirstName { get; set; }
        [Required]
        public string DisplayLastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{1,6}$",
            ErrorMessage = "Password Must Have Minimum six characters, at least one uppercase letter, one lowercase letter, one number and one special character:")]
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
