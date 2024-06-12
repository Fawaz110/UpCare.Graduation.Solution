namespace Core.UpCareEntities.BillEntities
{
    public class Bill : BaseEntity
    {
        public string FK_PayorId { get; set; }
        public string DeliveredService { get; set; }
        public decimal PaidMoney { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        
    }
}
