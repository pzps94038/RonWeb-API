using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Enum;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;

namespace RonWeb.API.Helper.ArticleCategory
{
    public class ArticleCategoryHelper : IArticleCategoryHelper
    {
        public async Task CreateAsync(CreateArticleCategoryRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var hasData = await srv.Query<Database.Models.ArticleCategory>()
                .FirstOrDefaultAsync(a => a.CategoryName == data.CategoryName);
            if (hasData != null)
            {
                throw new UniqueException();
            }
            var label = new Database.Models.ArticleCategory()
            {
                CategoryName = data.CategoryName,
                CreateDate = DateTime.Now
            };
            var indexModel = new CreateIndexModel<Database.Models.ArticleCategory>(
                Builders<Database.Models.ArticleCategory>.IndexKeys.Ascending("CategoryName"),
                new CreateIndexOptions { Unique = true }
            );
            var collection = srv.GetCollection<Database.Models.ArticleCategory>();
            await collection.Indexes.CreateOneAsync(indexModel);
            await srv.CreateAsync(label);
        }

        public async Task DeleteAsync(string data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<Database.Models.ArticleCategory>.Filter.Eq(a => a.Id, data);
            await srv.DeleteAsync(filter);
        }

        public async Task<List<Category>> GetListAsync()
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var list = await srv.Query<Database.Models.ArticleCategory>()
                .Select(a => new Category()
                {
                    CategoryId = a.Id,
                    CategoryName = a.CategoryName
                }).ToListAsync();
            return list;
        }

        public async Task UpdateAsync(string id, Category data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<Database.Models.ArticleCategory>.Filter.Eq(a => a.Id, id);
            var update = Builders<Database.Models.ArticleCategory>.Update.Set(a => a.CategoryName, data.CategoryName);
            await srv.UpdateAsync(filter, update);
        }
    }
}
