using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByGuidAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        IQueryable<T> GetAll();
        Task<List<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
