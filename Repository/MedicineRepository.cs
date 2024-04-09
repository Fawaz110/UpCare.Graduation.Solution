using Core.Repositories.Contract;
using Core.UpCareEntities;
using Repository.UpCareData;

namespace Repository
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(UpCareDbContext context) : base(context)
        {
        }

        
    }
}
