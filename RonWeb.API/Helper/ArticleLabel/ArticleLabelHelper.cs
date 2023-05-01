using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using RonWeb.Database.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.ArticleLabel;
using static MongoDB.Driver.WriteConcern;

namespace RonWeb.API.Helper.ArticleLabel
{
    public class ArticleLabelHelper: IArticleLabelHelper
    {
        public async Task CreateAsync(CreateArticleLabelRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var hasData = await srv.Query<RonWeb.Database.Models.ArticleLabel>()
                .FirstOrDefaultAsync(a => a.LabelName == data.LabelName);
            if (hasData != null)
            {
                throw new UniqueException();
            }
            var label = new RonWeb.Database.Models.ArticleLabel()
            {
                LabelName = data.LabelName,
                CreateDate = DateTime.Now
            };
            var indexModel = new CreateIndexModel<RonWeb.Database.Models.ArticleLabel>(
                Builders<RonWeb.Database.Models.ArticleLabel>.IndexKeys.Ascending("LabelName"),
                new CreateIndexOptions { Unique = true }
            );
            var collection = srv.GetCollection<RonWeb.Database.Models.ArticleLabel>();
            await collection.Indexes.CreateOneAsync(indexModel);
            await srv.CreateAsync(label);
        }

        public async Task<List<Label>> GetListAsync()
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var list = await srv.Query<RonWeb.Database.Models.ArticleLabel>()
                .Select(a=> new Label()
                {
                    LabelId = a.Id,
                    LabelName = a.LabelName
                }).ToListAsync();
            return list;
        }

        public async Task UpdateAsync(Label data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<RonWeb.Database.Models.ArticleLabel>.Filter.Eq(a => a.Id, data.LabelId);
            var update = Builders<RonWeb.Database.Models.ArticleLabel>.Update.Set(a => a.LabelName, data.LabelName);
            await srv.UpdateAsync<RonWeb.Database.Models.ArticleLabel>(filter, update);
        }

        public async Task DeleteAsync(string data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<RonWeb.Database.Models.ArticleLabel>.Filter.Eq(a => a.Id, data);
            await srv.DeleteAsync<RonWeb.Database.Models.ArticleLabel>(filter);
        }
    }
}

