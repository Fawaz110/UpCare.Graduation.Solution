using Core.Entities.UpCareEntities;

namespace UpCare.DTOs
{
    public class CheckupToDoDto
    {
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Checkup> Checkups { get; set; } = new List<Checkup>();
    }
}
