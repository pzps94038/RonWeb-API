using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using RonWeb.API.Models.CustomizeException;
using Microsoft.EntityFrameworkCore;
using RonWeb.Database.Redis;
using Newtonsoft.Json;
using RonWeb.Database.Entities;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.ArticleCategory;
using System.Collections.Generic;
using System.Linq;

namespace RonWeb.API.Helper.Search
{
    public class SearchHelper : ISearchHelper
    {
        private readonly RonWebDbContext _db;

        public SearchHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<KeywordeResponse> Category(long id, int? page)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category == null)
            {
                throw new NotFoundException();
            }
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var query = _db.Article.Where(a => a.CategoryId == id && a.Flag == "Y");
            // 獲取分頁Id
            var idList = await query
                .OrderByDescending(a => a.CreateDate)
                .Skip(skip)
                .Take(pageSize)
                .Select(a => a.ArticleId)
                .ToListAsync();
            // 獲取總數
            var total = await query.CountAsync();
            // 獲取文章標籤
            var articleLabelList = await _db.ArticleLabelMapping
                .Join(_db.ArticleLabel, a => a.LabelId, b => b.LabelId, (a, b) => new
                {
                    a.ArticleId,
                    a.LabelId,
                    b.LabelName,
                    a.CreateDate
                })
                .Where(a => idList.Contains(a.ArticleId))
                .ToListAsync();
            // 獲取文章列表
            var articleList = (await query.Where(a => idList.Contains(a.ArticleId)).ToListAsync())
                .Select(a => new ArticleItem()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = category.CategoryName,
                    ViewCount = a.ViewCount,
                    Flag = a.Flag,
                    CreateDate = a.CreateDate,
                    Labels = articleLabelList.Where(label => label.ArticleId == a.ArticleId)
                        .Distinct()
                        .Select(a => new Label(a.LabelId, a.LabelName, a.CreateDate))
                        .ToList()
                })
                .OrderByDescending(a => a.CreateDate)
                .ToList();
            var data = new KeywordeResponse()
            {
                Total = total,
                Articles = articleList,
                Keyword = category.CategoryName
            };
            return data;
        }

        public async Task<KeywordeResponse> Label(long id, int? page)
        {
            var label = await _db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
            if (label == null)
            {
                throw new NotFoundException();
            }
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var query = _db.Article
                .Join(_db.ArticleLabelMapping, a => a.ArticleId, b => b.ArticleId, (a, b) => new
                {
                    a.ArticleId,
                    b.LabelId,
                    a.CreateDate
                })
                .Where(a => a.LabelId == id)
                .Distinct();
            // 獲取分頁Id
            var idList = await query
                .OrderByDescending(a => a.CreateDate)
                .Skip(skip)
                .Take(pageSize)
                .Select(a => a.ArticleId)
                .ToListAsync();
            // 獲取總數
            var total = await query.CountAsync();
            // 獲取文章分類
            var articleCategoryList = await _db.ArticleCategory.Where(a => idList.Contains(a.CategoryId))
                .ToListAsync();
            // 獲取文章標籤
            var articleLabelList = await _db.ArticleLabelMapping
                .Join(_db.ArticleLabel, a => a.LabelId, b => b.LabelId, (a, b) => new
                {
                    a.ArticleId,
                    a.LabelId,
                    b.LabelName,
                    a.CreateDate
                })
                .Where(a => idList.Contains(a.ArticleId))
                .ToListAsync();
            // 獲取文章列表
            var articleList = (await _db.Article.Where(a => idList.Contains(a.ArticleId)).ToListAsync())
                .Select(a => new ArticleItem()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = articleCategoryList.FirstOrDefault(category => a.CategoryId == category.CategoryId)?.CategoryName ?? "",
                    ViewCount = a.ViewCount,
                    Flag = a.Flag,
                    CreateDate = a.CreateDate,
                    Labels = articleLabelList.Where(a => a.ArticleId == a.ArticleId)
                        .Distinct()
                        .Select(a => new Label(a.LabelId, a.LabelName, a.CreateDate))
                        .ToList()
                })
                .OrderByDescending(a => a.CreateDate)
                .ToList();
            var data = new KeywordeResponse()
            {
                Total = total,
                Articles = articleList,
                Keyword = label.LabelName
            };
            return data;
        }
    }
}

