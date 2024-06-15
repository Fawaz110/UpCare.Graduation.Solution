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

        public void UpdatePatintBookRoom(PatientBookRoom data)
            => _context.Set<PatientBookRoom>().Update(data);

        public async Task<PatientBookRoom> GetSpecificRecord(PatientBookRoom data)
            => await _context.Set<PatientBookRoom>().Where(x => x.FK_DoctorId == data.FK_DoctorId
                                                             && x.FK_PatientId == data.FK_PatientId
                                                             && x.StartDate == data.StartDate).FirstOrDefaultAsync();

        public async Task<PatientBookRoom> GetSpecificBookingAsync(string patientId, int roomId)
            => await _context.Set<PatientBookRoom>().FirstOrDefaultAsync(x => x.FK_PatientId == patientId && x.FK_RoomId == roomId);
    }
}
