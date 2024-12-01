using JobBoard.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoard.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly JobBoardContext _context;
        private readonly DbSet<T> dbSet;

        public Repository(JobBoardContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
             dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await dbSet.AnyAsync(predicate);

        public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(Guid id) => await dbSet.FindAsync(id);

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
