using MongoDB.Driver; // import the MongoDB driver library
using Play.Catalog.Service.Entities; // import the entity class

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "Items"; // the name of the MongoDB collection
        private readonly IMongoCollection<Item> dbCollection; // the MongoDB collection of items
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter; // used to create filter definitions for MongoDB queries

        public ItemsRepository()
        {
            var mongoclient = new MongoClient("mongodb://localhost:27017"); // connect to MongoDB on the local machine
            var database = mongoclient.GetDatabase("Catalog"); // get the Catalog database
            dbCollection = database.GetCollection<Item>(collectionName); // get the Items collection from the database
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync(); // find all items in the collection and return them
        }

        public async Task<Item> GetAsync(Guid id)
        {
            var filter = filterBuilder.Eq(entity => entity.Id, id); // create a filter that matches the item with the specified id
            return await dbCollection.Find(filter).FirstOrDefaultAsync(); // find the first item that matches the filter and return it
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity)); // check if the item is null and throw an exception if it is
            }

            await dbCollection.InsertOneAsync(entity); // insert the item into the collection
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity)); // check if the item is null and throw an exception if it is
            }

            var filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id); // create a filter that matches the existing item with the same id as the updated item
            await dbCollection.ReplaceOneAsync(filter, entity); // replace the existing item with the updated item in the collection
        }

        public async Task RemoveAsync(Guid id)
        {
            var filter = filterBuilder.Eq(entity => entity.Id, id); // create a filter that matches the item with the specified id
            await dbCollection.DeleteOneAsync(filter); // delete the item from the collection
        }
    }
}
