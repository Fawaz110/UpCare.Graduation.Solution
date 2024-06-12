using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UpCareEntities;

namespace UpCare.DTOs.BillDtos
{
    public class BillToAddDto
    {
        public string FK_PayorId { get; set; }
        public string DeliveredService { get; set; }
        public string PaymentIntentId { get; set; }
        public int PrescriptionId { get; set; }
        public PrescriptionPayment PrescriptionPayment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
