using Core.UpCareEntities;
using Microsoft.AspNetCore.Identity;

namespace UpCare.DTOs.MessageDtos
{
    public class MessagePackageToReturn
    {
        public IdentityUser Client { get; set; }
        public List<MessageToReturnDto> Messages { get; set; } = new List<MessageToReturnDto>();
    }
}
