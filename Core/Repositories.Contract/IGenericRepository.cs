using Core.Specifications;
using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdWithSpecsAsync(ISpecifications<TEntity> spec);
        Task<ICollection<TEntity>> GetAllWithSpecsAsync(ISpecifications<TEntity> spec);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Add(TEntity entity);
    }
}
