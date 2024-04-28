using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Doctor> _doctorManager;

        public PrescriptionService(
            IUnitOfWork unitOfWork,
            IPrescriptionRepository prescriptionRepository,
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager)
        {
            _unitOfWork = unitOfWork;
            _prescriptionRepository = prescriptionRepository;
            _patientManager = patientManager;
            _doctorManager = doctorManager;
        }

        public async Task<List<Prescription>> GetAllPrescriptionAsync(string? doctorId = null, string? patientId = null)
        {
            var data = (await _prescriptionRepository.GetAllAsync()).ToList();

            if (doctorId != null)
            {
                var doctor = await _doctorManager.FindByIdAsync(doctorId);

                if(doctor is null)
                    return new List<Prescription>();

                data = data.Where(x => x.FK_DoctorId == doctorId).ToList();
            }

            if(patientId != null)
            {
                var patient = await _patientManager.FindByIdAsync(patientId);

                if(patient is null)
                    return new List<Prescription>();

                data = data.Where(x =>x.FK_PatientId == patientId).ToList();
            }

            return data;
        }

        public async Task<List<CheckupInPrescription>> GetCheckupByPrescriptionIdAsync(int id)
            => await _prescriptionRepository.GetCheckupByPrescriptionIdAsync(id);

        public async Task<List<MedicineInPrescription>> GetMedicineByPrescriptionIdAsync(int id)
            => await _prescriptionRepository.GetMedicineByPrescriptionIdAsync(id);

        public async Task<List<RadiologyInPrescription>> GetRadiologyByPrescriptionIdAsync(int id)
            => await _prescriptionRepository.GetRadiologyByPrescriptionIdAsync(id);
    }
}
