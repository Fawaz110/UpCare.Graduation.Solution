using Core.UpCareEntities;

namespace Core.Entities.UpCareEntities
{
    public class Checkup : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
