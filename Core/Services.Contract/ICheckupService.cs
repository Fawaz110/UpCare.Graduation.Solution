using Core.Entities.UpCareEntities;
using Core.Specifications;

namespace Core.Services.Contract
{
    public interface ICheckupService
    {
        Task<ICollection<Checkup>> GetAllAsync();
        Task Update(Checkup entity);
        Task<int> DeleteAsync(int id);
        Task<Checkup> AddAsync(Checkup entity);
        Task<Checkup> GetByIdAsync(int id);
        Task<Checkup> GetByName(string name);
    }
}
