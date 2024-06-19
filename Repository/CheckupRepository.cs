using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.UpCareEntities;
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

        public async Task AddPatientResult(PatientCheckup patientCheckup)
            => await _context.AddAsync(patientCheckup);

        public async Task<List<PatientCheckup>> GetAllResults()
            => await _context.Set<PatientCheckup>().ToListAsync();

        public async Task<Checkup> GetByName(string name)
            => await _context.Checkups.FirstOrDefaultAsync(c => c.Name == name);
    }
}
