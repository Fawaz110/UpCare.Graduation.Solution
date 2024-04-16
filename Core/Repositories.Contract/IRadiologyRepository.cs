using Core.Entities.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IRadiologyRepository : IGenericRepository<Radiology>
    {
        Task<Radiology> GetByNameAsync(string name);
    }
}
