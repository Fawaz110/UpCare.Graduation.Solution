namespace UpCare.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public Core.UpCareUsers.Patient Patient { get; set; }
        public decimal Rate { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
