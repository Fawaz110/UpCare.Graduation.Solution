using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class PatientRoomRepository : IPatientRoomRepository
    {
        private readonly UpCareDbContext _context;

        public PatientRoomRepository(
            UpCareDbContext context)
        {
            _context = context;
        }

        public async Task AddBookingRoomAsync(PatientBookRoom data)
            => await _context.Set<PatientBookRoom>().AddAsync(data);

        public async Task<List<PatientBookRoom>> GetAllPatientRoomsAsync()
            => await _context.Set<PatientBookRoom>().ToListAsync();
    }
}
