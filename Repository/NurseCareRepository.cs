using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class NurseCareRepository : INurseCareRepository
    {
        private readonly UpCareDbContext _context;

        public NurseCareRepository(UpCareDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(NurseCare nurseCare)
            => await _context.Set<NurseCare>().AddAsync(nurseCare);

        public async Task DeleteAsync(NurseCare nurseCare)
            => _context.Set<NurseCare>().Remove(nurseCare);

        public async Task<List<NurseCare>> GetAllRecordsAsync()
            => await _context.Set<NurseCare>().ToListAsync();

        public async Task<List<NurseCare>> GetByNurseIdAsync(string nurseId)
            => await _context.Set<NurseCare>().Where(x=>x.FK_NurseId == nurseId).ToListAsync();

        public async Task<List<NurseCare>> GetByPatientIdAsync(string patientId)
            => await _context.Set<NurseCare>().Where(x=>x.FK_PatientId == patientId).ToListAsync();

        public async Task<List<NurseCare>> GetByRoomIdAsync(int roomId)
            => await _context.Set<NurseCare>().Where(x=>x.FK_RoomId == roomId).ToListAsync();

        public async Task<NurseCare> GetSpecificAsync(NurseCare nurseCare)
            => await _context.Set<NurseCare>().FirstOrDefaultAsync(x => x.FK_PatientId == nurseCare.FK_PatientId
                                                                     && x.FK_NurseId == nurseCare.FK_NurseId
                                                                     && x.FK_RoomId == nurseCare.FK_RoomId
                                                                     && x.DateTime == nurseCare.DateTime);

        public async Task UpdateAsync(NurseCare nurseCare)
            => _context.Set<NurseCare>().Update(nurseCare);
    }
}
