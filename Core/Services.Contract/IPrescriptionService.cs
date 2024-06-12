using Core.Entities.UpCareEntities;
using Core.UpCareEntities;
using Core.UpCareEntities.PrescriptionEntities;

namespace Core.Services.Contract
{
    public interface IPrescriptionService
    {
        Task<List<Prescription>> GetAllPrescriptionAsync(string? doctorId = null, string? patientId = null);
        Task<List<MedicineInPrescription>> GetMedicineByPrescriptionIdAsync(int id);
        Task<List<CheckupInPrescription>> GetCheckupByPrescriptionIdAsync(int id);
        Task<List<RadiologyInPrescription>> GetRadiologyByPrescriptionIdAsync(int id);
        Task<Prescription> GetByIdAsync(int id);
        Task<Prescription> AddPrescriptionAsync(Prescription prescription, 
                                                List<Medicine>? medicines = null, 
                                                List<Checkup>? checkups = null, 
                                                List<Radiology>? radiologies = null);
    }
}
