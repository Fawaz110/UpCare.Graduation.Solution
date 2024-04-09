using Core.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IMedicineService
    {
        Task<Medicine> GetByIdAsync(int id);

        Task<IEnumerable<Medicine>> GetAllAsync();

        Task<int> AddMedicine(Medicine entity);

        Task<Medicine> GetMedicineByName(string name);
    }
}
