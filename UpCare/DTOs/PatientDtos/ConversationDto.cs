using Core.UpCareUsers;

namespace UpCare.DTOs.PatientDtos
{
    public class ConversationDto
    {
        public Doctor Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public bool Passed { get; set; }
        public string Type { get; set; }
    }
}
