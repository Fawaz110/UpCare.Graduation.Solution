using System.ComponentModel.DataAnnotations;

namespace Core.UpCareEntities
{
    public class Message : BaseEntity
    {
        [Required]
        public string SenderId { get; set; } 
        [Required]
        public string ReceiverId { get; set; } 
        [Required]
        public string Content { get; set; } 
        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
