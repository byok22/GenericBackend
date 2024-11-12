using System.Linq.Expressions;
using Shared.Response;


namespace Domain.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByUuidAsync(string UuId);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task<DBResponse> UpdateAsync(TEntity entity);
        Task<DBResponse> RemoveAsync(TEntity entity);
    }
}
