namespace UpCare.DTOs.BillDtos
{
    public class BillDto
    {
        public string DeliveredService { get; set; }
        public decimal PaidMoney { get; set; }
        public DateTime DateTime { get; set; }
        public Core.UpCareUsers.Patient Payor { get; set; }
    }
}
