using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class OperationRepository : IOperationRepository
    {
        private readonly UpCareDbContext _context;

        public OperationRepository(
            UpCareDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DoctorDoOperation doctorDoOperation)
            => await _context.Set<DoctorDoOperation>().AddAsync(doctorDoOperation);

        public async Task<List<DoctorDoOperation>> GetScheduledOperationsAsync()
            => await _context.Set<DoctorDoOperation>().ToListAsync();
    }
}
