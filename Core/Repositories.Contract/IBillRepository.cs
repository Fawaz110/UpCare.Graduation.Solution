using Core.Entities.UpCareEntities;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;

namespace Core.Repositories.Contract
{
    public interface IBillRepository
    {
        Task<List<Medicine>> GetMedicineByBillId(int billId);
        Task<List<Radiology>> GetRadiologyByBillId(int billId);
        Task<List<Checkup>> GetCheckupByBillId(int billId);
        Task AddMedicineToBillAsync(MedicineInBill medicineInBill);
        Task AddCheckupToBillAsync(CheckupInBill checkupInBill);
        Task AddRadiologyToBillAsync(RadiologyInBill radiologyInBill);
    }
}
