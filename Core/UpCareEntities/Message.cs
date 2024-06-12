using System.ComponentModel.DataAnnotations;

namespace Core.UpCareEntities
{
    public enum MessagerRole
    {
        Admin, 
        Patient, 
        Doctor 
    };
    public class Message : BaseEntity
    {
        [Required]
        public string SenderId { get; set; } 
        public MessagerRole SenderRole { get; set; }
        [Required]
        public string ReceiverId { get; set; } 
        public MessagerRole ReceiverRole { get; set; }
        [Required]
        public string Content { get; set; } 
        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
