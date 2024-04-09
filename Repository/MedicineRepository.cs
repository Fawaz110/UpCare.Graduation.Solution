using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        private readonly UpCareDbContext _context;

        public MedicineRepository(UpCareDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Medicine> GetMedicineByNameAsync(string name)
        {
            var medicine = await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Name == name);

            return medicine;
        }
    }
}
