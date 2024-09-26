using MongoDB.Driver;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using Microsoft.EntityFrameworkCore;
using RonWeb.Database.MySql.RonWeb.DataBase;
using Newtonsoft.Json;

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
            var data = await _db.Article.Include(a => a.ArticleCategory)
                    .Include(a => a.ArticleLabelMapping)
                    .ThenInclude(a => a.ArticleLabel)
                    .Select(a => new GetByIdArticleResponse()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle,
                        PreviewContent = a.PreviewContent,
                        Content = a.Content,
                        CategoryId = a.CategoryId,
                        CategoryName = a.ArticleCategory.CategoryName,
                        ViewCount = a.ViewCount,
                        CreateDate = a.CreateDate,
                        Flag = a.Flag,
                        Labels = a.ArticleLabelMapping
                            .Select(mapping => new Label
                            {
                                LabelId = mapping.ArticleLabel.LabelId,
                                LabelName = mapping.ArticleLabel.LabelName
                            })
                            .ToList(),
                        References = a.ArticleReferences.Select(a => a.Link).ToList()
                    }).SingleOrDefaultAsync(a => a.ArticleId == id && a.Flag == "Y");

            if (data != null)
            {
                // 找到下一篇文章
                var nextArticle = await _db.Article.Where(a => a.CreateDate > data.CreateDate && a.Flag == "Y")
                    .OrderBy(a => a.CreateDate)
                    .Take(1)
                    .Select(a => new BlogPagination()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle
                    }).FirstOrDefaultAsync();
                // 找到上一篇文章
                var prevArticle = await _db.Article.Where(a => a.CreateDate < data.CreateDate && a.Flag == "Y")
                    .OrderByDescending(a => a.CreateDate)
                    .Take(1)
                    .Select(a => new BlogPagination()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle
                    }).FirstOrDefaultAsync();

                data.NextArticle = nextArticle;
                data.PrevArticle = prevArticle;
                var json = JsonConvert.SerializeObject(data);
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
            var query = _db.Article.Include(a => a.ArticleCategory)
                    .Include(a => a.ArticleLabelMapping)
                    .ThenInclude(a => a.ArticleLabel)
                    .Where(a => a.Flag == "Y")
                    .Select(a => new ArticleItem()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle,
                        PreviewContent = a.PreviewContent,
                        CategoryId = a.CategoryId,
                        CategoryName = a.ArticleCategory.CategoryName,
                        Flag = a.Flag,
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
                    ;
            if (keyword != null)
            {
                keyword = keyword.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(a => a.ArticleTitle.Contains(keyword) ||
                        a.PreviewContent.Contains(keyword) ||
                        a.CategoryName.Contains(keyword)
                    );
                }
            }
            query = query.OrderByDescending(a => a.CreateDate);
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
            var data = new GetArticleResponse()
            {
                Total = total,
                Articles = list
            };
            var json = JsonConvert.SerializeObject(data);
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