using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly UpCareDbContext _context;

        public ConsultationRepository(
            UpCareDbContext context)
        {
            _context = context;
        }
        public async Task AddConsultationAsync(PatientConsultation patientConsultation)
            => await _context.Set<PatientConsultation>().AddAsync(patientConsultation);

        public void DeleteAsync(PatientConsultation patientConsultation)
            => _context.Set<PatientConsultation>().Remove(patientConsultation);

        public async Task<List<PatientConsultation>> GetAllAsync()
            => await _context.Set<PatientConsultation>().OrderByDescending(x=>x.DateTime).ToListAsync();

        public async Task<List<PatientConsultation>> GetByDoctorIdAsync(string doctorId)
            => await _context.Set<PatientConsultation>().Where(pc => pc.FK_DoctorId == doctorId).ToListAsync();

        public async Task<List<PatientConsultation>> GetByPatientIdAsync(string patientId)
            => await _context.Set<PatientConsultation>().Where(pc => pc.FK_PatientId == patientId).ToListAsync();

        public async Task<List<PatientConsultation>> GetConsultationBetweenPatientAndDoctorAsync(string patientId, string doctorId)
            => await _context.Set<PatientConsultation>().Where(pc => pc.FK_DoctorId == doctorId 
                                                                  && pc.FK_PatientId == patientId).ToListAsync();

        public async Task<PatientConsultation> GetNextConsultationAsync(string patientId, string doctorId)
            => await _context.Set<PatientConsultation>().FirstOrDefaultAsync(pc => pc.FK_PatientId == patientId
                                                                                && pc.FK_DoctorId == doctorId
                                                                                && pc.DateTime > DateTime.UtcNow);

        public async Task<PatientConsultation> GetWithSpec(PatientConsultation consultation)
            => await _context.Set<PatientConsultation>().FirstOrDefaultAsync(x => x.FK_PatientId == consultation.FK_PatientId
                                                                               && x.FK_DoctorId == consultation.FK_DoctorId
                                                                               && x.DateTime == consultation.DateTime);
    }
}
