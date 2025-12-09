using System.Linq.Expressions;
using Shared.Response;


namespace Domain.Repositories
{
    /// <summary>
    /// Interface for a generic repository that provides basic CRUD operations for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets an entity by its UUID.
        /// </summary>
        /// <param name="UuId">The UUID of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
        Task<TEntity> GetByUuidAsync(string UuId);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Finds entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities that match the predicate.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response indicating the result of the update operation.</returns>
        Task<DBResponse> UpdateAsync(TEntity entity);

        /// <summary>
        /// Removes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response indicating the result of the remove operation.</returns>
        Task<DBResponse> RemoveAsync(TEntity entity);
    }
      
    
}
