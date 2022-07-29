using MongoDB.Driver;
using Play.Common;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Play.Common
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterDefinitionBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            dbCollection = _mongoDatabase.GetCollection<T>(typeof(T).Name);
        }



        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await dbCollection.Find(filterDefinitionBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync();

        }
        public async Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();

        }
        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(x => x.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(x => x.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }



    }
}