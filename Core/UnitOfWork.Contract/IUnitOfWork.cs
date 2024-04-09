using Core.Repositories.Contract;
using Core.UpCareEntities;

namespace Core.UnitOfWork.Contract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> CompleteAsync();
    }
}
