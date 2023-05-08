﻿using System;
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
                .Join(category, a=> a.CategoryId, b=> b._id, (a,b)=> new
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
                .SingleOrDefaultAsync(a => a.ArticleId == ObjectId.Parse(id));
            if (data == null)
            {
                throw new NotFoundException();
            }
            var label = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var lists = await srv.Query<ArticleLabelMapping>()
                 .Where(a => a.ArticleId == data.ArticleId)
                 .Join(label, a => a.LabelId, b => b._id, (a, b) => new 
                 {
                     LabelId = a.LabelId,
                     LabelName = b.LabelName
                 })
                .ToListAsync();
            var viewCount = data.ViewCount + 1;
            var filter = Builders<Database.Models.Article>.Filter.Eq(a => a._id, ObjectId.Parse(id));
            var update = Builders<Database.Models.Article>.Update
                .Set(a => a.ViewCount, viewCount);
            await srv.UpdateAsync(filter, update);
            var result = new GetByIdArticleResponse()
            {
                ArticleId = data.ArticleId.ToString(),
                ArticleTitle = data.ArticleTitle,
                PreviewContent = data.PreviewContent,
                Content = data.Content,
                CategoryId = data.CategoryId.ToString(),
                CategoryName = data.CategoryName,
                ViewCount = data.ViewCount,
                Labels = lists.Select(a=> new Label() { LabelId = a.LabelId.ToString(), LabelName = a.LabelName }).ToList(),
                CreateDate = data.CreateDate
            };
            return result;
        }

        public async Task<GetArticleResponse> GetListAsync(int? page)
        {
            var result = new List<ArticleItem>();
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var article = srv.Query<Article>();
            var category = srv.Query<RonWeb.Database.Models.ArticleCategory>();
            var label = srv.Query<RonWeb.Database.Models.ArticleLabel>();
            var mapping = srv.Query<ArticleLabelMapping> ();
            var c = article.Join(category, a => a.CategoryId, b => b._id, (a, b) => new
            {
                a._id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            }).ToList();
            var query = article.Join(category, a=> a.CategoryId, b=> b._id, (a,b)=> new
            {
                a._id,
                a.ArticleTitle,
                a.CategoryId,
                b.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
            }).Join(mapping, a => a._id, b => b.ArticleId, (a, b) => new
            {
                a._id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.Content,
                a.CreateDate,
                a.ViewCount,
                b.LabelId
            }).Join(label, a => a.LabelId, b => b._id, (a, b) => new
            {
                a._id,
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
            var group = query.Select(a=> new
            {
                a._id,
                a.ArticleTitle,
                a.CategoryId,
                a.CategoryName,
                a.PreviewContent,
                a.CreateDate,
                a.LabelId,
                a.LabelName
                
            }).GroupBy(a => new
            {
                a._id,
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
                    ArticleId = item.Key._id.ToString(),
                    ArticleTitle = item.Key.ArticleTitle,
                    CategoryId = item.Key.CategoryId.ToString(),
                    CategoryName = item.Key.CategoryName,
                    PreviewContent = item.Key.PreviewContent,
                    CreateDate = item.Key.CreateDate,
                    Labels = item.Select(a => new Label() { LabelId = a.LabelId.ToString(), LabelName = a.LabelName }).ToList()
                };
                result.Add(articleItem);
            }
            return new GetArticleResponse()
            {
                Total = total,
                Articles = result
            };
        }

        public async Task UpdateAsync(string id, UpdateArticleRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            using (var session = await srv.client.StartSessionAsync()) 
            {
                try
                {
                    session.StartTransaction();
                    var filter = Builders<Database.Models.Article>.Filter.Eq(a => a._id, ObjectId.Parse(id));
                    var update = Builders<Database.Models.Article>.Update
                        .Set(a => a.ArticleTitle, data.ArticleTitle)
                        .Set(a => a.Content, HtmlEncoder.Default.Encode(data.Content))
                        .Set(a => a.PreviewContent, HtmlEncoder.Default.Encode(data.PreviewContent))
                        .Set(a => a.CategoryId, ObjectId.Parse(data.CategoryId))
                        .Set(a => a.UpdateDate, DateTime.Now);
                    await srv.UpdateAsync(filter, update);
                    // 首先刪除原來的文章標籤
                    var mappingFilter = Builders<Database.Models.ArticleLabelMapping>.Filter.Eq(a => a.ArticleId, ObjectId.Parse(id));
                    await srv.DeleteManyAsync(mappingFilter);
                    // 新增當前文章的標籤
                    var mappingLabels = data.Labels.Select(a => new ArticleLabelMapping()
                    {
                        LabelId = ObjectId.Parse(a.LabelId),
                        ArticleId = ObjectId.Parse(id),
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
            var filter = Builders<Database.Models.Article>.Filter.Eq(a => a._id, ObjectId.Parse(data));
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
                    session.StartTransaction();
                    await srv.Query<RonWeb.Database.Models.ArticleCategory>().SingleAsync(a => a._id == ObjectId.Parse(data.CategoryId));
                    var article = new RonWeb.Database.Models.Article()
                    {
                        ArticleTitle = data.ArticleTitle,
                        Content = HtmlEncoder.Default.Encode(data.Content),
                        PreviewContent = HtmlEncoder.Default.Encode(data.PreviewContent),
                        CategoryId = ObjectId.Parse(data.CategoryId),
                        ViewCount = 0,
                        CreateDate = DateTime.Now,
                        CreateBy = data.CreateBy
                    };
                    await srv.CreateAsync(article);
                    var mappings = data.Labels.Select(a => new RonWeb.Database.Models.ArticleLabelMapping()
                    {
                        ArticleId = article._id,
                        LabelId = ObjectId.Parse(a.LabelId),
                        CreateDate = DateTime.Now
                    }).ToList();
                    if (mappings.Count > 0)
                    {
                        await srv.CreateManyAsync(mappings);
                    }
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