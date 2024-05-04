namespace Core.UpCareEntities.BillEntities
{
    public class Bill : BaseEntity
    {
        public string FK_Payor { get; set; }
        public string DeliveredService { get; set; }
        public decimal PaidMoney { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
