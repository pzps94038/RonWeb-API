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
using RonWeb.API.Models.CustomizeException;
using System.Collections.Generic;
using RonWeb.API.Models.Shared;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        public async Task<GetByIdArticleResponse> GetAsync(string id)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var category = srv.Query<Database.Models.ArticleCategory>();
            var data = await srv.Query<Article>()
                .Join(category, a=> a.CategoryId, b=> b.Id, (a,b)=> new GetByIdArticleResponse()
                {
                    ArticleId = a.Id,
                    ArticleTitle = a.ArticleTitle,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = b.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                })
                .SingleOrDefaultAsync(a => a.ArticleId == id);
            if (data == null)
            {
                throw new NotFoundException();
            }
            var label = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var lists = await srv.Query<ArticleLabelMapping>()
                 .Where(a => a.ArticleId == data.ArticleId)
                 .Join(label, a => a.LabelId, b => b.Id, (a, b) => new Label()
                 {
                     LabelId = a.LabelId,
                     LabelName = b.LabelName
                 })
                .ToListAsync();
            data.Labels = lists;
            var viewCount = data.ViewCount + 1;
            var filter = Builders<Database.Models.Article>.Filter.Eq(a => a.Id, id);
            var update = Builders<Database.Models.Article>.Update
                .Set(a => a.ViewCount, viewCount);
            await srv.UpdateAsync(filter, update);
            return data;
        }

        public async Task<List<ArticleItem>> GetListAsync(int limit, int offset, OrderEnum order, string? keyword)
        {
            var result = new List<ArticleItem>();
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var category = srv.Query<RonWeb.Database.Models.ArticleCategory>();
            var label = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var mapping = srv.Query<ArticleLabelMapping> ();
            var query = article.Join(category, a=> a.CategoryId, b=> b.Id, (a,b)=> new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            }).Join(mapping, a => a.Id, b => b.ArticleId, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.Content,
                a.CreateDate,
                a.ViewCount,
                b.LabelId
            }).Join(label, a => a.LabelId, b => b.Id, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.Content,
                a.CreateDate,
                a.ViewCount,
                a.LabelId,
                b.LabelName
            });

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(a =>
                    a.ArticleTitle.Contains(keyword) ||
                    a.Content.Contains(keyword) ||
                    a.CategoryName.Contains(keyword) ||
                    a.LabelName.Contains(keyword)
                );
            }
            var group = query.Select(a=> new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                Description = a.Content.Substring(0, 150),
                a.CreateDate,
                a.LabelId,
                a.LabelName
                
            }).GroupBy(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.Description,
                a.CreateDate,
            });

            switch (order)
            {
                case OrderEnum.TA:
                    group = group.OrderBy(a => a.Key.CreateDate);
                    break;
                case OrderEnum.TD:
                    group = group.OrderByDescending(a => a.Key.CreateDate);
                    break;
                default:
                    throw new NotFoundException();
            }
            var groupList = await group.Skip(offset).Take(limit).ToListAsync();
            foreach (var item in groupList)
            {
                var articleItem = new ArticleItem()
                {
                    ArticleId = item.Key.Id,
                    ArticleTitle = item.Key.ArticleTitle,
                    CategoryId = item.Key.CategoryId,
                    CategoryName = item.Key.CategoryName,
                    Description = item.Key.Description,
                    CreateDate = item.Key.CreateDate,
                    Labels = item.Select(a => new Label() { LabelId = a.LabelId, LabelName = a.LabelName }).ToList()
                };
                result.Add(articleItem);
            }
            return result;
        }

        public async Task UpdateAsync(string id, UpdateArticleRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            using (var session = await srv.client.StartSessionAsync()) 
            {
                try
                {
                    var filter = Builders<Database.Models.Article>.Filter.Eq(a => a.Id, id);
                    var update = Builders<Database.Models.Article>.Update
                        .Set(a => a.ArticleTitle, data.ArticleTitle)
                        .Set(a => a.Content, HtmlEncoder.Default.Encode(data.Content))
                        .Set(a => a.CategoryId, data.CategoryId)
                        .Set(a => a.UpdateDate, DateTime.Now);
                    await srv.UpdateAsync(filter, update);
                    // 首先刪除原來的文章標籤
                    var mappingFilter = Builders<Database.Models.ArticleLabelMapping>.Filter.Eq(a => a.ArticleId, id);
                    await srv.DeleteManyAsync(mappingFilter);
                    // 新增當前文章的標籤
                    var mappingLabels = data.Labels.Select(a => new ArticleLabelMapping()
                    {
                        LabelId = a.LabelId,
                        ArticleId = id,
                        CreateDate = DateTime.Now,
                    }).ToList();
                    await srv.CreateManyAsync(mappingLabels);
                    await session.CommitTransactionAsync();
                }
                catch 
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(string data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var filter = Builders<Database.Models.Article>.Filter.Eq(a => a.Id, data);
            await srv.DeleteAsync(filter);
        }

        public async Task CreateAsync(CreateArticleRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            using (var session = await srv.client.StartSessionAsync())
            {
                try
                {
                    var article = new RonWeb.Database.Models.Article()
                    {
                        ArticleTitle = data.ArticleTitle,
                        Content = HtmlEncoder.Default.Encode(data.Content),
                        CategoryId = data.CategoryId,
                        ViewCount = 0,
                        CreateDate = DateTime.Now,
                        CreateBy = data.CreateBy
                    };
                    await srv.CreateAsync(article);
                    var mappings = data.Labels.Select(a => new RonWeb.Database.Models.ArticleLabelMapping()
                    {
                        ArticleId = article.Id,
                        LabelId = a.LabelId,
                        CreateDate = DateTime.Now
                    }).ToList();
                    await srv.CreateManyAsync(mappings);
                    await session.CommitTransactionAsync();
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
        }
    }
}