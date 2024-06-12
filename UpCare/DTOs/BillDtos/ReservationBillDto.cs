using Core.Services.Contract;

namespace UpCare.DTOs.BillDtos
{
    public class ReservationBillDto
    {
        public int Id { get; set; }
        public string DeliveredService { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public Core.UpCareUsers.Patient Payor { get; set; }
    }
}
