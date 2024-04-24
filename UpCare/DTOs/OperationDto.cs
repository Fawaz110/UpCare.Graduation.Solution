using Core.UpCareUsers;

namespace UpCare.DTOs
{
    public class OperationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Admin Admin { get; set; }
    }
}
