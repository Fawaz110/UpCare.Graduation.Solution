using Core.UpCareEntities;

namespace Core.Entities.UpCareEntities
{
    public class Operation : BaseEntity 
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string FK_AdminId { get; set; }
    }
}
