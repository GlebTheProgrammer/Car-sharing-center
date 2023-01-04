using CarSharingApp.Domain.Primitives;
using System.Linq.Expressions;

namespace CarSharingApp.Domain.Abstractions
{
    public interface IRepository<T> where T : Entity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<IReadOnlyCollection<T>> GetAsyncWithLimit(Expression<Func<T, bool>> filter, int skip, int limit);
        Task<IReadOnlyCollection<T>> GetAsyncWithLimit(int skip, int limit);
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}
