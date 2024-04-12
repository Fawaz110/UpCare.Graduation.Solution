using Core.Repositories.Contract;
using Core.Specifications;
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

        public async Task Add(TEntity entity)
            => await _context.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);
        
        
        public async Task<ICollection<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<ICollection<TEntity>> GetAllWithSpecsAsync(ISpecifications<TEntity> spec)
            => await ApplySpecifications(spec).ToListAsync();

        public async Task<TEntity> GetByIdAsync(int id)
            => await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<TEntity> GetByIdWithSpecsAsync(ISpecifications<TEntity> spec)
           => await ApplySpecifications(spec).FirstOrDefaultAsync();

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity> spec)
        {
            return SpecificationsEvaluator<TEntity>.GetQuery(_context.Set<TEntity>(), spec);
        }
    }
}
