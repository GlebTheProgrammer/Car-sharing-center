using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Primitives;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CarSharingApp.Infrastructure.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : Entity
    {
        private readonly IMongoCollection<T> collection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);
        }

        public async Task CreateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }    

            await collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await collection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAsyncWithLimit(Expression<Func<T, bool>> filter, int skip, int limit)
        {
            return await collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAsyncWithLimit(int skip, int limit)
        {
            return await collection.Find(filterBuilder.Empty).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, entity.Id);

            await collection.ReplaceOneAsync(filter, entity);
        }
    }
}
