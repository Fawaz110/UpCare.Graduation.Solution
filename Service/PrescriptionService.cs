using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
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


        public async Task<Prescription> AddPrescriptionAsync(Prescription prescription, List<Medicine>? medicines = null, List<Checkup>? checkups = null, List<Radiology>? radiologies = null)
        {
            await _unitOfWork.Repository<Prescription>().Add(prescription);

            var result = await _unitOfWork.CompleteAsync();

            if (medicines != null && medicines.Count() != 0)
                foreach (var medicine in medicines)
                {
                    var itemToAdd = new MedicineInPrescription
                    {
                        FK_PrescriptionId = prescription.Id,
                        FK_MedicineId = medicine.Id
                    };

                    await _prescriptionRepository.AddMedicineToPrescriptionAsync(itemToAdd);
                }

            if (checkups != null && checkups.Count() != 0)
                foreach (var checkup in checkups)
                {
                    var itemToAdd = new CheckupInPrescription
                    {
                        FK_PrescriptionId = prescription.Id,
                        FK_CheckupId = checkup.Id
                    };

                    await _prescriptionRepository.AddCheckupToPrescriptionAsync(itemToAdd);
                }

            if (radiologies != null && radiologies.Count() != 0)
                foreach (var radiology in radiologies)
                {
                    var itemToAdd = new RadiologyInPrescription
                    {
                        FK_PrescriptionId = prescription.Id,
                        FK_RadiologyId = radiology.Id
                    };

                    await _prescriptionRepository.AddRadiologyToPrescriptionAsync(itemToAdd);
                }

            result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return prescription;
        }
        
        public async Task<List<Prescription>> GetAllPrescriptionAsync(string? doctorId = null, string? patientId = null)
        {
            var data = (await _prescriptionRepository.GetAllAsync()).ToList();

            if (doctorId != null)
            {
                var doctor = await _doctorManager.FindByIdAsync(doctorId);

                if(doctor != null)
                    data = data.Where(x => x.FK_DoctorId == doctorId).ToList();
            }

            if(patientId != null)
            {
                var patient = await _patientManager.FindByIdAsync(patientId);

                if(patient != null)
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

        public async Task<Prescription> GetByIdAsync(int id)
            => await _unitOfWork.Repository<Prescription>().GetByIdAsync(id);
    }
}
