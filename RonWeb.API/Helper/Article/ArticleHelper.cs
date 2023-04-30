using System;
using MongoDB.Driver;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using RonWeb.Core;
using RonWeb.Database.Models;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using MongoDB.Driver.Linq;
using RonWeb.API.Enum;

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        public async Task<ArticleModel> GetByIdAsync(string id)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var category = srv.Query<ArticleCategory>(MongoTableEnum.ArticleCategory.Description());
            var data = await srv.Query<Article>(MongoTableEnum.Article.Description())
                .Where(a => a.ArticleId == id)
                .Join(category, a=> a.CategoryId, b=> b.CategoryId, (a,b)=> new ArticleModel()
                {
                    ArticleId = a.ArticleId!,
                    ArticleTitle = a.ArticleTitle,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = b.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                    CreateBy = a.CreateBy,
                    UpdateDate = a.UpdateDate,
                    UpdateBy = a.UpdateBy
                })
                .SingleAsync();
            var label = srv.Query<ArticleLabel>(MongoTableEnum.ArticleLabel.Description());
            var lists = await srv.Query<ArticleLabelMapping>(MongoTableEnum.ArticleLabelMapping.Description())
                .Where(a => a.ArticleId == data.ArticleId)
                 .Join(label, a => a.LabelId, b => b.LabelId, (a, b) => new LabelModel()
                 {
                     LabelId = a.LabelId,
                     LabelName = b.LabelName
                 })
                .ToListAsync();
            data.Labels = lists;
            return data;
        }
    }
}