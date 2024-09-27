using MongoDB.Driver;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RonWeb.Database.Entities;
using System.Reflection.Emit;
using System.Linq;

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
            var vwArticle = await _db.VwArticle.Where(a => a.ArticleId == id && a.Flag == "Y")
                        .GroupBy(a => a.ArticleId)
                        .SingleOrDefaultAsync();
            if (vwArticle != null)
            {
                var article = vwArticle.First();
                var data = new GetByIdArticleResponse()
                {
                    ArticleId = article.ArticleId,
                    ArticleTitle = article.ArticleTitle,
                    PreviewContent = article.PreviewContent,
                    Content = article.Content,
                    CategoryId = article.CategoryId,
                    CategoryName = article.CategoryName ?? "",
                    ViewCount = article.ViewCount,
                    Flag = article.Flag,
                    Labels = vwArticle.Where(a => a.LabelId != null)
                        .Select(a => new Models.Shared.Label()
                        {
                            LabelId = article.LabelId,
                            LabelName = article.LabelName,
                            CreateDate = article.LabelCreateDate
                        })
                        .ToList(),
                    References = vwArticle.Where(a => a.Link != null).Select(a => a.Link!).ToList(),
                    CreateDate = article.ArticleCreateDate
                };
                // 找到下一篇文章
                var nextArticle = await _db.Article.Where(a => a.CreateDate > data.CreateDate && a.Flag == "Y")
                    .OrderBy(a => a.CreateDate)
                    .Select(a => new BlogPagination()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle
                    })
                    .FirstOrDefaultAsync();
                // 找到上一篇文章
                var prevArticle = await _db.Article.Where(a => a.CreateDate < data.CreateDate && a.Flag == "Y")
                    .OrderByDescending(a => a.CreateDate)
                    .Select(a => new BlogPagination()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle
                    })
                    .FirstOrDefaultAsync();

                data.NextArticle = nextArticle;
                data.PrevArticle = prevArticle;
                return data;
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<GetArticleResponse> GetListAsync(int? page, string? keyword)
        {
            var curPage = page.GetValueOrDefault(1);
            var query = _db.VwArticle.Where(a => a.Flag == "Y");
            if (keyword != null)
            {
                keyword = keyword.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(a => a.ArticleTitle.Contains(keyword) ||
                        a.PreviewContent.Contains(keyword) ||
                        (a.CategoryName != null && a.CategoryName.Contains(keyword))
                    );
                }
            }
            var idQuery = query.Select(a => new { a.ArticleId, a.ArticleCreateDate })
                .Distinct()
                .OrderByDescending(a => a.ArticleCreateDate)
                .AsQueryable();
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            idQuery = skip == 0 ? idQuery.Take(pageSize) : idQuery.Skip(skip).Take(pageSize);
            var idList = idQuery.Select(a => a.ArticleId).ToList();
            var group = query.GroupBy(a => a.ArticleId);
            var total = group.Count();
            List<ArticleItem> list = (await _db.VwArticle.Where(a => idList.Contains(a.ArticleId)).ToListAsync())
             .GroupBy(a => a.ArticleId)
             .Select(a => new ArticleItem()
             {
                 ArticleId = a.Key,
                 ArticleTitle = a.First().ArticleTitle,
                 PreviewContent = a.First().PreviewContent,
                 Content = a.First().Content,
                 CategoryId = a.First().CategoryId,
                 CategoryName = a.First().CategoryName ?? "",
                 ViewCount = a.First().ViewCount,
                 Flag = a.First().Flag,
                 CreateDate = a.First().ArticleCreateDate,
                 Labels = a.Select(article => new Models.Shared.Label()
                 {
                     LabelId = article.LabelId,
                     LabelName = article.LabelName,
                     CreateDate = article.LabelCreateDate
                 })
                .OrderBy(label => label.CreateDate)
                .GroupBy(label => label.LabelId)
                .Select(g => g.First())
                .ToList()
             })
             .ToList();
            var data = new GetArticleResponse()
            {
                Total = total,
                Articles = list
            };
            return data;
        }

        public async Task UpdateArticleViews(long id)
        {
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                article.ViewCount = article.ViewCount + 1;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}