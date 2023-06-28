using MongoDB.Driver;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using RonWeb.Database.MySql.RonWeb.DataBase;
using RonWeb.Database.MySql.RonWeb.Table;
using RonWeb.Database.Redis;
using Newtonsoft.Json;

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        private readonly RonWebDbContext db;
        private readonly Task<RedisConnection> _redisConnectionFactory;
        private readonly TimeSpan _cacheSpan = TimeSpan.FromHours(1);

        public ArticleHelper(RonWebDbContext dbContext, Task<RedisConnection> redisConnectionFactory)
        {
            this.db = dbContext;
            this._redisConnectionFactory = redisConnectionFactory;
        }

        public async Task<GetByIdArticleResponse> GetAsync(long id)
        {
            var redisConection = await this._redisConnectionFactory;
            var key = RedisKeys.ArticleByIdPrefix + id.ToString();
            var cache = await redisConection.BasicRetryAsync(async (db) => await db.StringGetAsync(key));
            if (cache.HasValue)
            {
                return JsonConvert.DeserializeObject<GetByIdArticleResponse>(cache.ToString())!;
            }
            else
            {
                var data = await db.Article.Include(a => a.ArticleCategory)
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
                            .ToList()
                    }).SingleOrDefaultAsync(a => a.ArticleId == id && a.Flag == "Y");

                if (data != null)
                {
                    // 找到下一篇文章
                    var nextArticle = await db.Article.Where(a => a.CreateDate > data.CreateDate)
                        .OrderBy(a => a.CreateDate)
                        .Take(1)
                        .Select(a => new BlogPagination()
                        {
                            ArticleId = a.ArticleId,
                            ArticleTitle = a.ArticleTitle
                        }).FirstOrDefaultAsync();
                    // 找到上一篇文章
                    var prevArticle = await db.Article.Where(a => a.CreateDate < data.CreateDate)
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
                    await redisConection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, json, _cacheSpan));
                    return data;
                }
                else
                {
                    throw new NotFoundException();
                }
            }
        }

        public async Task<GetArticleResponse> GetListAsync(int? page, string? keyword)
        {
            var redisConection = await this._redisConnectionFactory;
            var curPage = page.GetValueOrDefault(1);
            string key;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                key = @$"{RedisKeys.ArticlePagePrefix}{curPage.ToString()}:{keyword}";
            }
            else
            {
                key = @$"{RedisKeys.ArticlePagePrefix}{curPage.ToString()}";
            }
            var cache = await redisConection.BasicRetryAsync(async (db) => await db.StringGetAsync(key));
            if (cache.HasValue)
            {
                return JsonConvert.DeserializeObject<GetArticleResponse>(cache.ToString())!;
            }
            else
            {
                var query = db.Article.Include(a => a.ArticleCategory)
                    .Include(a => a.ArticleLabelMapping)
                    .ThenInclude(a => a.ArticleLabel)
                    .Where(a=> a.Flag == "Y")
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
                await redisConection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, json, _cacheSpan));
                return data;
            }
        }

        public async Task UpdateArticleViews(long id)
        {
            var article = await db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                article.ViewCount = article.ViewCount + 1;
                await db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}