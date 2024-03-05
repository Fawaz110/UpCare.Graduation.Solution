namespace Core.UpCareEntities
{
    public class Feedback : BaseEntity
    {
        public string FK_PatientId { get; set; }
        public decimal Rate { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
