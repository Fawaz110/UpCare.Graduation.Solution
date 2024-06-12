using Core.Entities.UpCareEntities;

namespace UpCare.DTOs.ZonesDtos
{
    public class CheckupZoneInPrescriptionDto
    {
        public int FK_PrescriptionId { get; set; }
        public decimal Total { get; set; }
        public DateTime DateTime { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Checkup> Checkups { get; set; }
    }
}
