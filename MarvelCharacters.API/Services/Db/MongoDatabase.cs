using MarvelCharacters.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelCharacters.API.Services.Db
{
    public class MongoDatabase
    {
        private const string CONNECTION = "";
        private const string CHARACTER_COLLECTION_NAME = "character";

        static IMongoDatabase GetDatabase(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(database);
        }

        /// <inheritdoc />
        public async Task<Character> AddLike(Character data)
        {
            var collection = GetDatabase(CONNECTION, "Marvel")
                .GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            await collection.InsertOneAsync(data, new InsertOneOptions());

            return data;
        }

        /// <inheritdoc />
        public Task RemoveLike(int id)
        {
            var collection = GetDatabase(CONNECTION, "Marvel")
                .GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            return collection.DeleteOneAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<Character>> GetLikes()
        {
            var collection = GetDatabase(CONNECTION, "Marvel")
                .GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            return await collection.AsQueryable().ToListAsync();
        }
    }
}

