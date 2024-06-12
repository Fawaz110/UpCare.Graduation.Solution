using Core.UpCareEntities.PrescriptionEntities;

namespace Core.Repositories.Contract
{
    public interface IPrescriptionRepository : IGenericRepository<Prescription>
    {
        #region Add For Prescription
        Task AddPrescriptionAsync(Prescription prescription);
        Task AddCheckupToPrescriptionAsync(CheckupInPrescription checkupInPrescription);
        Task AddMedicineToPrescriptionAsync(MedicineInPrescription medicineInPrescription);
        Task AddRadiologyToPrescriptionAsync(RadiologyInPrescription radiologyInPrescription);
        #endregion

        #region Get Prescription Data
        Task<List<Prescription>> GetPrescriptionsByDoctorIdAsync(string id);
        Task<List<Prescription>> GetPrescriptionsByPatientIdAsync(string id);
        Task<List<MedicineInPrescription>> GetMedicineByPrescriptionIdAsync(int id);
        Task<List<RadiologyInPrescription>> GetRadiologyByPrescriptionIdAsync(int id);
        Task<List<CheckupInPrescription>> GetCheckupByPrescriptionIdAsync(int id);
        //Task<List<DoctorGivePrescription>> GetPrescriptionsByDoctorIdAsync(string id);
        //Task<List<DoctorGivePrescription>> GetPrescriptionsByPatientIdAsync(string id);
        #endregion

    }
}
