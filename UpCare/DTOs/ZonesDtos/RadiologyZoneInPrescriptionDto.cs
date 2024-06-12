using Core.Entities.UpCareEntities;

namespace UpCare.DTOs.ZonesDtos
{
    public class RadiologyZoneInPrescriptionDto
    {
        public int FK_PrescriptionId { get; set; }
        public decimal Total { get; set; }
        public DateTime DateTime { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Radiology> Radiologies { get; set; }
    }
}
