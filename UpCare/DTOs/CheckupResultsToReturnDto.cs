using Core.Entities.UpCareEntities;

namespace UpCare.DTOs
{
    public class CheckupResultsToReturnDto
    {
        public Checkup Checkup { get; set; }
        public string ResultUrl { get; set; }
        public DateTime DateTime { get; set; }
    }
}
