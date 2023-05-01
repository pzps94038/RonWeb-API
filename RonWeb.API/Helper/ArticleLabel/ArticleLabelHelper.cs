using RonWeb.API.Enum;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.ArticleLabel;

namespace RonWeb.API.Helper.ArticleLabel
{
    public class ArticleLabelHelper : IArticleLabelHelper
    {
        public async Task CreateAsync(CreateArticleLabelRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var hasData = await srv.Query<Database.Models.ArticleLabel>()
                .FirstOrDefaultAsync(a => a.LabelName == data.LabelName);
            if (hasData != null)
            {
                throw new UniqueException();
            }
            var label = new Database.Models.ArticleLabel()
            {
                LabelName = data.LabelName,
                CreateDate = DateTime.Now
            };
            var indexModel = new CreateIndexModel<Database.Models.ArticleLabel>(
                Builders<Database.Models.ArticleLabel>.IndexKeys.Ascending("LabelName"),
                new CreateIndexOptions { Unique = true }
            );
            var collection = srv.GetCollection<Database.Models.ArticleLabel>();
            await collection.Indexes.CreateOneAsync(indexModel);
            await srv.CreateAsync(label);
        }

        public async Task<List<Label>> GetListAsync()
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var list = await srv.Query<Database.Models.ArticleLabel>()
                .Select(a=> new Label()
                {
                    LabelId = a.Id,
                    LabelName = a.LabelName
                }).ToListAsync();
            return list;
        }

        public async Task UpdateAsync(string id, Label data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<Database.Models.ArticleLabel>.Filter.Eq(a => a.Id, id);
            var update = Builders<Database.Models.ArticleLabel>.Update.Set(a => a.LabelName, data.LabelName);
            await srv.UpdateAsync(filter, update);
        }

        public async Task DeleteAsync(string data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<Database.Models.ArticleLabel>.Filter.Eq(a => a.Id, data);
            await srv.DeleteAsync(filter);
        }
    }
}

