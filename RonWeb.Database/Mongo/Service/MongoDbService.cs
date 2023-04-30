using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
namespace RonWeb.Database.Service
{
    public class MongoDbService
    {
        public IMongoClient client {
            get {
                if (MongoDbService._client == null)
                {
                    MongoDbService._client = new MongoClient(this._settings);
                    return MongoDbService._client;
                }
                else
                {
                    return MongoDbService._client;
                }
            }
        }
        private static IMongoClient? _client;
        public IMongoDatabase db
        {
            get
            {
                if (MongoDbService._db == null)
                {
                    MongoDbService._db = this.client.GetDatabase(this._dbName);
                    return MongoDbService._db;
                }
                else
                {
                    return MongoDbService._db;
                }
            }
        }
        private static IMongoDatabase? _db;
        private string _dbName;
        private MongoClientSettings _settings;
        public MongoDbService(string connectionUrl, string dbName)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionUrl);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            this._settings = settings;
            this._dbName = dbName;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return this.db.GetCollection<T>(collectionName);
        }

        public IMongoQueryable<T> Query<T>(string collectionName)
        {
            return this.GetCollection<T>(collectionName).AsQueryable();
        }

        public async Task CreateAsync<T>(string collectionName, T data)
        {
            await this.db.GetCollection<T>(collectionName).InsertOneAsync(data);
        }

        public async Task CreateManyAsync<T>(string collectionName, List<T> list)
        {
            await this.db.GetCollection<T>(collectionName).InsertManyAsync(list);
        }


        public async Task<UpdateResult> UpdateAsync<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return await this.db.GetCollection<T>(collectionName).UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateManyAsync<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return await this.db.GetCollection<T>(collectionName).UpdateManyAsync(filter, update);
        }

        public async Task<DeleteResult> DeleteAsync<T>(string collectionName, FilterDefinition<T> filter)
        {
            return await this.db.GetCollection<T>(collectionName).DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteManyAsync<T>(string collectionName, FilterDefinition<T> filter)
        {
            return await this.db.GetCollection<T>(collectionName).DeleteManyAsync(filter);
        }

        public async Task<string> CreateIndexAsync<T>(string collectionName, CreateIndexModel<T> data)
        {
           return await this.db.GetCollection<T>(collectionName).Indexes.CreateOneAsync(data);
        }

        public async Task<IEnumerable<string>> CreateManyIndexAsync<T>(string collectionName, List<CreateIndexModel<T>> list)
        {
            return await this.db.GetCollection<T>(collectionName).Indexes.CreateManyAsync(list);
        }
    }
}

