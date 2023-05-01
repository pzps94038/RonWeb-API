using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.Database.Mongo.MongoAttribute;

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

        public IMongoCollection<T> GetCollection<T>()
        {
            Type type = typeof(T);
            MongoAttribute tag = (MongoAttribute)Attribute.GetCustomAttribute(type, typeof(MongoAttribute))!;
            return this.db.GetCollection<T>(tag.TableName);
        }

        public IMongoQueryable<T> Query<T>()
        {
            return this.GetCollection<T>().AsQueryable();
        }

        public async Task CreateAsync<T>(T data)
        {
            await this.GetCollection<T>().InsertOneAsync(data);
        }

        public async Task CreateManyAsync<T>(List<T> list)
        {
            await this.GetCollection<T>().InsertManyAsync(list);
        }


        public async Task<UpdateResult> UpdateAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return await this.GetCollection<T>().UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateManyAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return await this.GetCollection<T>().UpdateManyAsync(filter, update);
        }

        public async Task<DeleteResult> DeleteAsync<T>(FilterDefinition<T> filter)
        {
            return await this.GetCollection<T>().DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteManyAsync<T>(FilterDefinition<T> filter)
        {
            return await this.GetCollection<T>().DeleteManyAsync(filter);
        }

        public async Task<string> CreateIndexAsync<T>(CreateIndexModel<T> data)
        {
           return await this.GetCollection<T>().Indexes.CreateOneAsync(data);
        }

        public async Task<IEnumerable<string>> CreateManyIndexAsync<T>(List<CreateIndexModel<T>> list)
        {
            return await this.GetCollection<T>().Indexes.CreateManyAsync(list);
        }
    }
}

