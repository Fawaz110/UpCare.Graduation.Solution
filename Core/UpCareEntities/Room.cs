using Core.UpCareEntities;

namespace Core.Entities.UpCareEntities
{
    public class Room : BaseEntity
    {
        public decimal PricePerNight { set; get; }
        public int NumberOfBeds { set; get; }
        public int AvailableBeds { set; get; }
        public string FK_ReceptionistId { set; get; }
    }
}
