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

namespace RonWeb.API.Helper.Search
{
    public class SearchHelper : ISearchHelper
    {
        public async Task<KeywordeResponse> Category(string id, int? page)
        {
            var result = new List<ArticleItem>();
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var category = await srv.Query<RonWeb.Database.Models.ArticleCategory>().SingleOrDefaultAsync(a=> a.Id == id);
            if (category == null)
            {
                throw new NotFoundException();
            }
            var categoryQuery = srv.Query<RonWeb.Database.Models.ArticleCategory>();
            var label = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var mapping = srv.Query<ArticleLabelMapping>();
            var query = article.Join(categoryQuery, a => a.CategoryId, b => b.Id, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            })
            .Where(a=> a.CategoryId == id)
            .Join(mapping, a => a.Id, b => b.ArticleId, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
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
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
                a.LabelId,
                b.LabelName
            });
            var group = query.Select(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
                a.LabelId,
                a.LabelName

            }).GroupBy(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
            });

            group = group.OrderByDescending(a => a.Key.CreateDate);
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var total = group.Count();
            var groupList = skip == 0 ? await group.Take(pageSize).ToListAsync() : await group.Skip(skip).Take(pageSize).ToListAsync();
            foreach (var item in groupList)
            {
                var articleItem = new ArticleItem()
                {
                    ArticleId = item.Key.Id,
                    ArticleTitle = item.Key.ArticleTitle,
                    CategoryId = item.Key.CategoryId,
                    CategoryName = item.Key.CategoryName,
                    PreviewContent = item.Key.PreviewContent,
                    CreateDate = item.Key.CreateDate,
                    Labels = item.Select(a => new Label() { LabelId = a.LabelId, LabelName = a.LabelName }).ToList()
                };
                result.Add(articleItem);
            }
            return new KeywordeResponse()
            {
                Total = total,
                Articles = result,
                Keyword = category.CategoryName
            };
        }

        public async Task<KeywordeResponse> Keyword(string keyword, int? page)
        {
            var result = new List<ArticleItem>();
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var category = srv.Query<RonWeb.Database.Models.ArticleCategory>();
            var label = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var mapping = srv.Query<ArticleLabelMapping>();
            var query = article.Join(category, a => a.CategoryId, b => b.Id, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            }).Join(mapping, a => a.Id, b => b.ArticleId, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
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
                a.PreviewContent,
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
                    a.PreviewContent.Contains(keyword) ||
                    a.Content.Contains(keyword) ||
                    a.CategoryName.Contains(keyword) ||
                    a.LabelName.Contains(keyword)
                );
            }
            var group = query.Select(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
                a.LabelId,
                a.LabelName

            }).GroupBy(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
            });

            group = group.OrderByDescending(a => a.Key.CreateDate);
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var total = group.Count();
            var groupList = skip == 0 ? await group.Take(pageSize).ToListAsync() : await group.Skip(skip).Take(pageSize).ToListAsync();
            foreach (var item in groupList)
            {
                var articleItem = new ArticleItem()
                {
                    ArticleId = item.Key.Id,
                    ArticleTitle = item.Key.ArticleTitle,
                    CategoryId = item.Key.CategoryId,
                    CategoryName = item.Key.CategoryName,
                    PreviewContent = item.Key.PreviewContent,
                    CreateDate = item.Key.CreateDate,
                    Labels = item.Select(a => new Label() { LabelId = a.LabelId, LabelName = a.LabelName }).ToList()
                };
                result.Add(articleItem);
            }
            return new KeywordeResponse()
            {
                Total = total,
                Articles = result,
                Keyword = keyword
            };
        }

        public async Task<KeywordeResponse> Label(string id, int? page)
        {
            var result = new List<ArticleItem>();
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var label = await srv.Query<RonWeb.Database.Models.ArticleLabel>().SingleOrDefaultAsync(a => a.Id == id);
            if (label == null)
            {
                throw new NotFoundException();
            }
            var category= srv.Query<RonWeb.Database.Models.ArticleCategory>();
            var labelQuery = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var mapping = srv.Query<ArticleLabelMapping>();
            var query = article.Join(category, a => a.CategoryId, b => b.Id, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            })
            .Where(a => a.CategoryId == id)
            .Join(mapping, a => a.Id, b => b.ArticleId, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
                b.LabelId
            }).Join(labelQuery, a => a.LabelId, b => b.Id, (a, b) => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
                a.LabelId,
                b.LabelName
            })
            .Where(a=> a.LabelId == id);
            var group = query.Select(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
                a.LabelId,
                a.LabelName

            }).GroupBy(a => new
            {
                a.Id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
            });

            group = group.OrderByDescending(a => a.Key.CreateDate);
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var total = group.Count();
            var groupList = skip == 0 ? await group.Take(pageSize).ToListAsync() : await group.Skip(skip).Take(pageSize).ToListAsync();
            foreach (var item in groupList)
            {
                var articleItem = new ArticleItem()
                {
                    ArticleId = item.Key.Id,
                    ArticleTitle = item.Key.ArticleTitle,
                    CategoryId = item.Key.CategoryId,
                    CategoryName = item.Key.CategoryName,
                    PreviewContent = item.Key.PreviewContent,
                    CreateDate = item.Key.CreateDate,
                    Labels = item.Select(a => new Label() { LabelId = a.LabelId, LabelName = a.LabelName }).ToList()
                };
                result.Add(articleItem);
            }
            return new KeywordeResponse()
            {
                Total = total,
                Articles = result,
                Keyword = label.LabelName
            };
        }
    }
}

