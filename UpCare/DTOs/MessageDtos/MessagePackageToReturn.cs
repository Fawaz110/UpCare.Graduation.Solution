using Core.UpCareEntities;
using Microsoft.AspNetCore.Identity;

namespace UpCare.DTOs.MessageDtos
{
    public class MessagePackageToReturn
    {
        public string ClientId { get; set; }
        public List<MessageToReturnDto> Messages { get; set; } = new List<MessageToReturnDto>();
    }
}
