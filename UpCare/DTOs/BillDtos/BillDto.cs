using Core.Entities.UpCareEntities;
using Core.UpCareEntities;

namespace UpCare.DTOs.BillDtos
{
    public class BillDto
    {
        public string DeliveredService { get; set; }
        public decimal PaidMoney { get; set; }
        public DateTime DateTime { get; set; }
        public Core.UpCareUsers.Patient Payor { get; set; }
        public List<Medicine> Medicines { get; set; }
        public List<Radiology> Radiologies { get; set; }
        public List<Checkup> Checkups { get; set; }

    }
}
