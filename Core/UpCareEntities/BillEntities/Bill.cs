namespace Core.UpCareEntities.BillEntities
{
    public class Bill : BaseEntity
    {
        public string DeliveredService { get; set; }
        public decimal PaidMoney { get; set; }
        public DateTime DateTime { get; set; }
    }
}
