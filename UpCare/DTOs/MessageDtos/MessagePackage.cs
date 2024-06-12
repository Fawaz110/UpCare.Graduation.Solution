using Core.UpCareEntities;

namespace UpCare.DTOs.MessageDtos
{
    public class MessagePackage
    {
        public string Id { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
