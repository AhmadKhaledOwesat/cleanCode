using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Base business logic layer class providing common CRUD operations for entities
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TId">Entity identifier type</typeparam>
    /// <typeparam name="TFilter">Filter type for search operations</typeparam>
    public partial class BaseBll<T,TId,TFilter>(IBaseDal<T,TId,TFilter> baseDal):IBaseBll<T,TId,TFilter> where T : BaseEntity<TId> where TId : struct
    {
        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public virtual async Task AddAsync(T entity) => await baseDal.AddAsync(entity);
        
        /// <summary>
        /// Retrieves all entities from the database
        /// </summary>
        /// <returns>List of all entities</returns>
        public virtual async Task<List<T>> GetAllAsync()=>await  baseDal.GetAllAsync();
        
        /// <summary>
        /// Retrieves entities based on search parameters with pagination
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching entities</returns>
        public virtual async Task<PageResult<T>> GetAllAsync(TFilter searchParameters)=>await baseDal.GetAllAsync(searchParameters);    
        
        /// <summary>
        /// Retrieves an entity by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <returns>The entity if found, otherwise null</returns>
        public virtual async Task<T> GetByIdAsync(TId id)=> await baseDal.GetByIdAsync(id);
        
        /// <summary>
        /// Updates an existing entity in the database
        /// </summary>
        /// <param name="entity">The entity to update</param>
        public virtual async Task UpdateAsync(T entity)=>  await baseDal.UpdateAsync(entity);
        
        /// <summary>
        /// Finds all entities that match the specified expression
        /// </summary>
        /// <param name="expression">Lambda expression to filter entities</param>
        /// <returns>List of entities matching the expression</returns>
        public virtual async Task<List<T>> FindAllByExpressionAsync(Expression<Func<T, bool>> expression) => await baseDal.FindAllByExpressionAsync(expression);
        
        /// <summary>
        /// Finds the first entity that matches the specified expression
        /// </summary>
        /// <param name="expression">Lambda expression to filter entities</param>
        /// <returns>The first matching entity, or null if not found</returns>
        public virtual async Task<T> FindByExpressionAsync(Expression<Func<T, bool>> expression) => await baseDal.FindByExpressionAsync(expression);
        
        /// <summary>
        /// Finds the last entity that matches the specified expression
        /// </summary>
        /// <param name="expression">Lambda expression to filter entities</param>
        /// <returns>The last matching entity, or null if not found</returns>
        public virtual async Task<T> FindLastByExpressionAsync(Expression<Func<T, bool>> expression) => await baseDal.FindLastByExpressionAsync(expression);
        
        /// <summary>
        /// Deletes multiple entities from the database
        /// </summary>
        /// <param name="entities">List of entities to delete</param>
        public virtual async Task DeleteRangeAsync(List<T> entities) => await baseDal.DeleteRangeAsync(entities);
        
        /// <summary>
        /// Adds multiple entities to the database in a single operation
        /// </summary>
        /// <param name="entities">List of entities to add</param>
        public virtual async Task AddRangeAsync(List<T> entities) => await baseDal.AddRangeAsync(entities);
        
        /// <summary>
        /// Updates multiple entities in the database in a single operation
        /// </summary>
        /// <param name="entities">List of entities to update</param>
        public virtual async Task UpdateRangeAsync(List<T> entities) => await baseDal.UpdateRangeAsync(entities);
        
        /// <summary>
        /// Deletes an entity by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete</param>
        /// <returns>True if the entity was deleted successfully, false otherwise</returns>
        public virtual async Task<bool> DeleteAsync(TId id) => await baseDal.DeleteAsync(id);
        
        /// <summary>
        /// Gets the count of entities that match the specified expression
        /// </summary>
        /// <param name="expression">Lambda expression to filter entities</param>
        /// <returns>The number of entities matching the expression</returns>
        public virtual async Task<int> GetCountByExpressionAsync(Expression<Func<T, bool>> expression) => await baseDal.GetCountByExpressionAsync(expression);

        public virtual async Task<bool> IsAuthorizedAsync(Guid permssionId) => await baseDal.IsAuthorizedAsync(permssionId);

    }
}
