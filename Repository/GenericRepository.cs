using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly UpCareDbContext _context;

        public GenericRepository(UpCareDbContext context)
        {
            _context = context;
        }

        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);
        
        public async Task<ICollection<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity> GetByIdAsync(int id)
            => await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);
    }
}
