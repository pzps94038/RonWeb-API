using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Models;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using RonWeb.API.Models.CustomizeException;
using MongoDB.Bson;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RonWeb.API.Helper.Search
{
    public class SearchHelper : ISearchHelper
    {
        public async Task<KeywordeResponse> Category(string id, int? page)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            ObjectId categoryId = new ObjectId();
            if (ObjectId.TryParse(id, out categoryId))
            {
                var category = await srv.Query<RonWeb.Database.Models.ArticleCategory>().SingleOrDefaultAsync(a => a._id == ObjectId.Parse(id));
                if (category == null)
                {
                    throw new NotFoundException();
                }
                var categoryQuery = srv.Query<RonWeb.Database.Models.ArticleCategory>();
                var query = article.Join(categoryQuery, a => a.CategoryId, b => b._id, (a, b) => new
                {
                    a._id,
                    a.ArticleTitle,
                    a.CategoryId,
                    b.CategoryName,
                    a.PreviewContent,
                    a.Content,
                    a.CreateDate,
                    a.ViewCount,
                })
                .Where(a => a.CategoryId == ObjectId.Parse(id))
                .OrderByDescending(a => a.CreateDate);
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
                return new KeywordeResponse()
                {
                    Total = total,
                    Articles = result,
                    Keyword = category.CategoryName
                };
            }
            else 
            {
                throw new NotFoundException();
            }
        }

        public async Task<KeywordeResponse> Keyword(string keyword, int? page)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var category = srv.Query<RonWeb.Database.Models.ArticleCategory>();
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
            });
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(a =>
                    a.ArticleTitle.Contains(keyword) ||
                    a.PreviewContent.Contains(keyword) ||
                    a.Content.Contains(keyword) ||
                    a.CategoryName.Contains(keyword)
                );
            }
            query = query.OrderByDescending(a => a.CreateDate);
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
            return new KeywordeResponse()
            {
                Total = total,
                Articles = result,
                Keyword = keyword
            };
        }
    }
}

