using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class CheckupRepository : GenericRepository<Checkup>, ICheckupRepository
    {
        private readonly UpCareDbContext _context;

        public CheckupRepository(UpCareDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Checkup> GetByName(string name)
            => await _context.Checkups.FirstOrDefaultAsync(c => c.Name == name);
    }
}
