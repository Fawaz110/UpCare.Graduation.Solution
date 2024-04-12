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

        public async Task<List<string>> GetCategories()
            => await _context.Set<Medicine>().Select(m => m.Category).Distinct().ToListAsync();

        public async Task<Medicine> GetMedicineByNameAsync(string name)
        {
            var medicine = await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Name == name);

            return medicine;
        }

        public async Task<List<Medicine>> SearchByMedicineName(string term)
            => await _context.Set<Medicine>().Where(m => m.Name.Trim().ToLower().Contains(term.Trim().ToLower())).ToListAsync();

        public async Task<List<Medicine>> GetShortage(int leastNormalQuantity)
            => await _context.Set<Medicine>().Where(m => m.Quantity <= leastNormalQuantity).ToListAsync();

        
    }
}
