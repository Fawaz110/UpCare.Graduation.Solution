using Core.UpCareEntities;

namespace Core.Entities.UpCareEntities
{
    public class Room : BaseEntity
    {
        public int Number { set; get; }
        public decimal PricePerNight { set; get; }
        public int NumberOfBeds { set; get; }
        public string FK_ReceptionistId { set; get; }
    }
}
