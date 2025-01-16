using MongoDB.Driver;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RonWeb.Database.Entities;
using System.Linq;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        private readonly RonWebDbContext _db;

        public ArticleHelper(
            RonWebDbContext dbContext
        )
        {
            _db = dbContext;
        }

        public async Task<GetByIdArticleResponse> GetAsync(long id)
        {
            var article = await _db.Article.Where(a => a.ArticleId == id && a.Flag == Flag.Y).SingleOrDefaultAsync();
            if (article == null)
            {
                throw new NotFoundException();
            }

            // 分類名稱
            var categoryName = (await _db.ArticleCategory.FirstOrDefaultAsync(a => a.CategoryId == article.CategoryId))?.CategoryName ?? "";

            // 文章標籤查詢
            var articleLabelQuery = _db.ArticleLabelMapping.Where(a => a.ArticleId == article.ArticleId);
            var articleLabels = await _db.ArticleLabel
                .Join(
                    articleLabelQuery,
                    a => a.LabelId,
                    b => b.LabelId,
                    (a, b) => new Label(a.LabelId, a.LabelName, b.CreateDate)
                )
                .ToListAsync();

            // 參考文章
            var references = await _db.ArticleReferences
                .Where(a => a.ArticleId == article.ArticleId)
                .Select(a => a.Link)
                .ToListAsync();

            // 找到下一篇文章
            var nextArticle = await _db.Article.Where(a => a.CreateDate > article.CreateDate && a.Flag == Flag.Y)
                .OrderBy(a => a.CreateDate)
                .Take(1)
                .Select(a => new BlogPagination()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle
                }).FirstOrDefaultAsync();
            // 找到上一篇文章
            var prevArticle = await _db.Article.Where(a => a.CreateDate < article.CreateDate && a.Flag == Flag.Y)
                .OrderByDescending(a => a.CreateDate)
                .Take(1)
                .Select(a => new BlogPagination()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle
                }).FirstOrDefaultAsync();

            // 回傳
            return new GetByIdArticleResponse()
            {
                ArticleId = article.ArticleId,
                ArticleTitle = article.ArticleTitle,
                PreviewContent = article.PreviewContent,
                Content = article.Content,
                CategoryId = article.CategoryId,
                CategoryName = categoryName,
                Flag = article.Flag,
                ViewCount = article.ViewCount,
                CreateDate = article.CreateDate,
                Labels = articleLabels,
                References = references,
                PrevArticle = prevArticle,
                NextArticle = nextArticle,
            };
        }

        public async Task<GetArticleResponse> GetListAsync(int? page, string? keyword)
        {
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;

            // 串接分類表
            var query = _db.Article
                .Join(
                    _db.ArticleCategory,
                    a => a.CategoryId,
                    b => b.CategoryId,
                    (a, b) => new
                    {
                        a.ArticleId,
                        a.ArticleTitle,
                        a.PreviewContent,
                        a.Content,
                        a.CategoryId,
                        b.CategoryName,
                        a.ViewCount,
                        a.Flag,
                        a.CreateDate,
                    })
                .AsQueryable();

            // 條件搜尋
            keyword = keyword?.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.ArticleTitle.Contains(keyword) ||
                    a.PreviewContent.Contains(keyword) ||
                    a.CategoryName.Contains(keyword)
                );
            }

            // 獲取分頁Id
            var idList = await query
                .Where(a => a.Flag == Flag.Y)
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
                    CategoryName = a.CategoryName,
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

            return new GetArticleResponse()
            {
                Total = total,
                Articles = articleList
            };
        }

        public async Task UpdateArticleViews(long id)
        {
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id && a.Flag == Flag.Y);
            if (article == null)
            {
                throw new NotFoundException();
            }
            article.ViewCount = article.ViewCount + 1;
            await _db.SaveChangesAsync();
        }
    }
}