using Core.Entities.UpCareEntities;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;

namespace Core.Services.Contract
{
    public interface IBillService
    {
        Task<Bill> AddAsync(Bill bill, List<MedicineInPrescription> medicineInPrescriptions, List<CheckupInPrescription> checkupInPrescriptions, List<RadiologyInPrescription> radiologyInPrescriptions);
        Task<Bill> AddAsync(Bill bill);
        Task<Bill> GetWithPaymentIntent(string paymentIntentId);
        Task<List<Medicine>> GetMedicineInBillAsync(int billId);
        Task<List<Radiology>> GetRadiologiesInBillAsync(int billId);
        Task<List<Checkup>> GetCheckupInBillAsync(int billId);
    }
}
