using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public IdentityUser Sender { get; set; }
        public IdentityUser Receiver { get; set; }
    }
}
