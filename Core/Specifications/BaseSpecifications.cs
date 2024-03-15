using Core.UpCareEntities;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : BaseEntity
    {
        public Expression<Func<TEntity, bool>> Craiteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();

        public BaseSpecifications()
        {

        }
        public BaseSpecifications(Expression<Func<TEntity, bool>> craiteria)
        {
            Craiteria = craiteria;
        }
    }
}
