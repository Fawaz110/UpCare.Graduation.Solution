using Core.Repositories.Contract;
using Core.UpCareEntities.PrescriptionEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        private readonly UpCareDbContext _context;

        public PrescriptionRepository(UpCareDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(Prescription prescription)
            => await _context.Set<Prescription>().AddAsync(prescription);

        public async Task AddCheckupToPrescriptionAsync(CheckupInPrescription checkupInPrescription)
            => await _context.Set<CheckupInPrescription>().AddAsync(checkupInPrescription);

        public async Task AddMedicineToPrescriptionAsync(MedicineInPrescription medicineInPrescription)
            => await _context.Set<MedicineInPrescription>().AddAsync(medicineInPrescription);

        public async Task AddRadiologyToPrescriptionAsync(RadiologyInPrescription radiologyInPrescription)
            => await _context.Set<RadiologyInPrescription>().AddAsync(radiologyInPrescription);

        public async Task<List<Prescription>> GetPrescriptionsByDoctorIdAsync(string id)
            => await _context.Set<Prescription>().Where(x => x.FK_DoctorId == id).ToListAsync();
            
        public async Task<List<Prescription>> GetPrescriptionsByPatientIdAsync(string id)
            => await _context.Set<Prescription>().Where(x => x.FK_PatientId == id).ToListAsync();

        public async Task<List<MedicineInPrescription>> GetMedicineByPrescriptionIdAsync(int id)
            => await _context.Set<MedicineInPrescription>().Where(x=>x.FK_PrescriptionId == id).ToListAsync();

        public async Task<List<RadiologyInPrescription>> GetRadiologyByPrescriptionIdAsync(int id)
            => await _context.Set<RadiologyInPrescription>().Where(x=>x.FK_PrescriptionId == id).ToListAsync();
        

        public async Task<List<CheckupInPrescription>> GetCheckupByPrescriptionIdAsync(int id)
            => await _context.Set<CheckupInPrescription>().Where(x=>x.FK_PrescriptionId == id).ToListAsync();
        

        //public async Task SetDoctorForPrescriptionAsync(DoctorGivePrescription doctorGivePrescription)
        //    => await _context.Set<DoctorGivePrescription>().AddAsync(doctorGivePrescription);

        //public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        //    => await _context.Set<Prescription>().FindAsync(id);

        //public async Task<List<DoctorGivePrescription>> GetPrescriptionsByDoctorIdAsync(string id)
        //    => await _context.Set<DoctorGivePrescription>().Where(X => X.FK_DoctorId == id).ToListAsync();

        //public async Task<List<DoctorGivePrescription>> GetPrescriptionsByPatientIdAsync(string id)
        //    => await _context.Set<DoctorGivePrescription>().Where(x =>x.FK_PatientId == id).ToListAsync();
    }
}
