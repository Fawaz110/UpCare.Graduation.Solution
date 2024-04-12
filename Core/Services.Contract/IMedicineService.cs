using Core.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IMedicineService
    {
        Task<Medicine> GetByIdAsync(int id);

        Task<IEnumerable<Medicine>> GetAllAsync();

        Task<int> AddMedicine(Medicine entity);

        Task<Medicine> GetMedicineByName(string name);

        Task<List<Medicine>> SearchByMedicineName(string term);

        Task<List<string>> GetCategories();

        Task<List<Medicine>> GetShortage(int leastNormalQuantity);

        void UpdateMedicine(Medicine entity);

        Task<int> DeleteAsync(int id);
    }
}
