using Core.Entities.UpCareEntities;
using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IBillRepository
    {
        Task<List<Medicine>> GetMedicineByBillId(int billId);
        Task<List<Radiology>> GetRadiologyByBillId(int billId);
        Task<List<Checkup>> GetCheckupByBillId(int billId);

    }
}
