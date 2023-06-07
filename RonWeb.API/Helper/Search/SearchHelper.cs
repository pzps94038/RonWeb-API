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
using RonWeb.Database.MySql.RonWeb.DataBase;
using Microsoft.EntityFrameworkCore;

namespace RonWeb.API.Helper.Search
{
    public class SearchHelper: ISearchHelper
    {
        public readonly RonWebDbContext db;

        public SearchHelper(RonWebDbContext dbContext)
        {
            this.db = dbContext;
        }

        public async Task<KeywordeResponse> Category(long id, int? page)
        {
            var category = await db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category == null)
            {
                throw new NotFoundException();
            }
            else
            {
                var query = db.Article.Include(a => a.ArticleCategory)
                .Include(a => a.ArticleLabelMapping)
                .ThenInclude(a => a.ArticleLabel)
                .Select(a => new ArticleItem()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    CategoryId = a.CategoryId,
                    CategoryName = a.ArticleCategory.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                    Labels = a.ArticleLabelMapping
                        .Select(mapping => new Label
                        {
                            LabelId = mapping.ArticleLabel.LabelId,
                            LabelName = mapping.ArticleLabel.LabelName
                        })
                        .ToList()
                })
                .Where(a => a.CategoryId == id)
                .OrderByDescending(a => a.CreateDate);
                var curPage = page.GetValueOrDefault(1);
                var pageSize = 10;
                var skip = (curPage - 1) * pageSize;
                var total = query.Count();
                List<ArticleItem> list;
                if (skip == 0)
                {
                    list = await query.Take(pageSize).ToListAsync();
                }
                else
                {
                    list = await query.Skip(skip).Take(pageSize).ToListAsync();
                }

                return new KeywordeResponse()
                {
                    Total = total,
                    Articles = list,
                    Keyword = category.CategoryName
                };
            }
        }

        public async Task<KeywordeResponse> Label(long id, int? page)
        {
            var label = await db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
            if (label == null)
            {
                throw new NotFoundException();
            }
            else
            {
                var query = db.Article.Include(a => a.ArticleCategory)
                .Include(a => a.ArticleLabelMapping)
                .ThenInclude(a => a.ArticleLabel)
                .Select(a => new ArticleItem()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    CategoryId = a.CategoryId,
                    CategoryName = a.ArticleCategory.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                    Labels = a.ArticleLabelMapping
                        .Select(mapping => new Label
                        {
                            LabelId = mapping.ArticleLabel.LabelId,
                            LabelName = mapping.ArticleLabel.LabelName
                        })
                        .ToList()
                })
                .Where(a => a.Labels.Any(b => b.LabelId == id))
                .OrderByDescending(a => a.CreateDate);
                var curPage = page.GetValueOrDefault(1);
                var pageSize = 10;
                var skip = (curPage - 1) * pageSize;
                var total = query.Count();
                List<ArticleItem> list;
                if (skip == 0)
                {
                    list = await query.Take(pageSize).ToListAsync();
                }
                else
                {
                    list = await query.Skip(skip).Take(pageSize).ToListAsync();
                }

                return new KeywordeResponse()
                {
                    Total = total,
                    Articles = list,
                    Keyword = label.LabelName
                };
            }
        }
    }
}

