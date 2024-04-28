using Core.UpCareEntities.PrescriptionEntities;

namespace Core.Services.Contract
{
    public interface IPrescriptionService
    {
        Task<List<Prescription>> GetAllPrescriptionAsync(string? doctorId = null, string? patientId = null);
        Task<List<MedicineInPrescription>> GetMedicineByPrescriptionIdAsync(int id);
        Task<List<CheckupInPrescription>> GetCheckupByPrescriptionIdAsync(int id);
        Task<List<RadiologyInPrescription>> GetRadiologyByPrescriptionIdAsync(int id);
    }
}
