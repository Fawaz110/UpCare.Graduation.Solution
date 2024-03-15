using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<ICollection<TEntity>> GetAllAsync();
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
