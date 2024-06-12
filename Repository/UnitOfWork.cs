using Core.Repositories.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Microsoft.Extensions.Logging;
using Repository.UpCareData;
using System.Collections;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UpCareDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(UpCareDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(key))
            {
                var instance = new GenericRepository<TEntity>(_context) /* as GenericRepository<BaseEntity> */;

                _repositories.Add(key, instance);
            }


            return _repositories[key] as IGenericRepository<TEntity>;
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();
    }
}
