using JobBoard.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JobBoard.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);

        Task<IEnumerable<JobCategory>> GetJobCategoriesAsync();

    }
}
