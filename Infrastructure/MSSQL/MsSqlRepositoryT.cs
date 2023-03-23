using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Primitives;
using CarSharingApp.Infrastructure.MSSQL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarSharingApp.Infrastructure.MSSQL
{
    public sealed class MsSqlRepository<T> : IRepository<T> where T : Entity
    {
        private readonly CarSharingAppContext _context;

        public MsSqlRepository(CarSharingAppContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Set<T>().AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            T? entity = await _context.Set<T>().FindAsync(id);

            if (entity is null)
                return;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(filter);
        }

        public async Task<IReadOnlyCollection<T>> GetAsyncWithLimit(Expression<Func<T, bool>> filter, int skip, int limit)
        {
            return await _context.Set<T>().Where(filter).Skip(skip).Take(limit).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAsyncWithLimit(int skip, int limit)
        {
            return await _context.Set<T>().Skip(skip).Take(limit).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
