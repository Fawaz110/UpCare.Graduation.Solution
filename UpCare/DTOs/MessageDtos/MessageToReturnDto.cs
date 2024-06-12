using Core.UpCareEntities;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs.MessageDtos
{
    public class MessageToReturnDto
    {
        public string SenderId { get; set; }
        public MessagerRole SenderRole { get; set; }
        public string ReceiverId { get; set; }
        public MessagerRole ReceiverRole { get; set; }
        public string Content { get; set; }
        public bool isSent { get; set; } = false;
        public DateTime DateTime { get; set; }
    }
}
