using Core.UpCareEntities;

namespace UpCare.DTOs.ZonesDtos
{
    public class PharmacyZoneInPrescription
    {
        public int FK_PrescriptionId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Total { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public List<Medicine> Medicines { get; set; }
    }
}
