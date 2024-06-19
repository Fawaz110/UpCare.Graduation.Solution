using Core.Entities.UpCareEntities;

namespace UpCare.DTOs
{
    public class RadiologyToDoDto
    {
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Radiology> Radiologies { get; set; } = new List<Radiology>();
    }
}
