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
using MongoDB.Bson;
using System.Security.Cryptography;
using Ganss.Xss;

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        public async Task<GetByIdArticleResponse> GetAsync(string id)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var category = srv.Query<Database.Models.ArticleCategory>();
            ObjectId objId = new ObjectId();
            if (ObjectId.TryParse(id, out objId))
            {
                var data = await srv.Query<Article>()
                .Join(category, a => a.CategoryId, b => b._id, (a, b) => new
                {
                    ArticleId = a._id,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = b.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                })
                .SingleOrDefaultAsync(a => a.ArticleId == objId);
                if (data == null)
                {
                    throw new NotFoundException();
                }
                var result = new GetByIdArticleResponse()
                {
                    ArticleId = data.ArticleId.ToString(),
                    ArticleTitle = data.ArticleTitle,
                    PreviewContent = data.PreviewContent,
                    Content = data.Content,
                    CategoryId = data.CategoryId.ToString(),
                    CategoryName = data.CategoryName,
                    ViewCount = data.ViewCount,
                    CreateDate = data.CreateDate
                };
                return result;
            }
            else 
            {
                throw new NotFoundException();
            }
        }

        public async Task<GetArticleResponse> GetListAsync(int? page)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var category = srv.Query<RonWeb.Database.Models.ArticleCategory>();
            // 撈文章
            var query = article.Join(category, a => a.CategoryId, b => b._id, (a, b) => new
            {
                a._id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            }).OrderByDescending(a => a.CreateDate);
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var total = query.Count();
            var list = skip == 0 ? await query.Take(pageSize).ToListAsync() : await query.Skip(skip).Take(pageSize).ToListAsync();
            var result = list.Select(a => new ArticleItem()
            {
                ArticleId = a._id.ToString(),
                ArticleTitle = a.ArticleTitle,
                PreviewContent = a.PreviewContent,
                CategoryId = a.CategoryId.ToString(),
                CategoryName = a.CategoryName,
                ViewCount = a.ViewCount,
                CreateDate = a.CreateDate,
            }).ToList();
            return new GetArticleResponse()
            {
                Total = 0,
                Articles = result
            };
        }

        public async Task UpdateAsync(string id, UpdateArticleRequest data)
        {
            ObjectId objId = new ObjectId();
            ObjectId categoryId = new ObjectId();
            if (ObjectId.TryParse(id, out objId) && (ObjectId.TryParse(data.CategoryId, out categoryId)))
            {
                string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
                var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
                using (var session = await srv.client.StartSessionAsync())
                {
                    try
                    {
                        session.StartTransaction();
                        var sanitizer = new HtmlSanitizer();
                        var filter = Builders<Database.Models.Article>.Filter.Eq(a => a._id, objId);
                        var update = Builders<Database.Models.Article>.Update
                            .Set(a => a.ArticleTitle, data.ArticleTitle)
                            .Set(a => a.Content, sanitizer.Sanitize(data.Content))
                            .Set(a => a.PreviewContent, sanitizer.Sanitize(data.PreviewContent))
                            .Set(a => a.CategoryId, categoryId)
                            .Set(a => a.UpdateDate, DateTime.Now)
                            .Set(a => a.UpdateBy, data.UserId);
                        await srv.UpdateAsync(filter, update);
                        await session.CommitTransactionAsync();
                    }
                    catch
                    {
                        await session.AbortTransactionAsync();
                        throw;
                    }
                }
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteAsync(string data)
        {
            ObjectId objId = new ObjectId();
            if (ObjectId.TryParse(data, out objId))
            {
                string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
                var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
                using (var session = await srv.client.StartSessionAsync())
                {
                    try
                    {
                        session.StartTransaction();
                        var filter = Builders<Database.Models.Article>.Filter.Eq(a => a._id, objId);
                        var imgFilter = Builders<Database.Models.ArticleImage>.Filter.Eq(a => a.ArticleId, objId);
                        await Task.WhenAll(srv.DeleteAsync(filter), srv.DeleteAsync(imgFilter));
                        await session.CommitTransactionAsync();
                    }
                    catch
                    {
                        await session.AbortTransactionAsync();
                        throw;
                    }
                }
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task CreateAsync(CreateArticleRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            using (var session = await srv.client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var sanitizer = new HtmlSanitizer();
                    await srv.Query<RonWeb.Database.Models.ArticleCategory>().SingleAsync(a => a._id == ObjectId.Parse(data.CategoryId));
                    var article = new RonWeb.Database.Models.Article()
                    {
                        ArticleTitle = data.ArticleTitle,
                        Content = sanitizer.Sanitize(data.Content),
                        PreviewContent = sanitizer.Sanitize(data.PreviewContent),
                        CategoryId = ObjectId.Parse(data.CategoryId),
                        ViewCount = 0,
                        CreateDate = DateTime.Now,
                        CreateBy = data.UserId
                    };
                    await srv.CreateAsync(article);
                    await session.CommitTransactionAsync();
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
        }

        public async Task UpdateArticleViews(string id)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var category = srv.Query<Database.Models.ArticleCategory>();
            ObjectId objId = new ObjectId();
            if (ObjectId.TryParse(id, out objId))
            {
                var data = await srv.Query<Article>().SingleOrDefaultAsync(a => a._id == objId);
                if (data == null)
                {
                    throw new NotFoundException();
                }
                var viewCount = data.ViewCount + 1;
                var filter = Builders<Database.Models.Article>.Filter.Eq(a => a._id, objId);
                var update = Builders<Database.Models.Article>.Update
                    .Set(a => a.ViewCount, viewCount);
                await srv.UpdateAsync(filter, update);
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}