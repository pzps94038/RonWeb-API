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

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        public async Task<GetByIdArticleResponse> GetAsync(long id)
        {
            using (var db = new RonWebDbContext())
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
                        Labels = a.ArticleLabelMapping
                            .Select(mapping => new Label
                            {
                                LabelId = mapping.ArticleLabel.LabelId,
                                LabelName = mapping.ArticleLabel.LabelName
                            })
                            .ToList()
                    }).SingleOrDefaultAsync(a => a.ArticleId == id);

                if (data != null)
                {
                    return data;
                }
                else
                {
                    throw new NotFoundException();
                }
            }
        }

        public async Task<GetArticleResponse> GetListAsync(int? page)
        {
            using (var db = new RonWebDbContext())
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
                    });
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

                return new GetArticleResponse()
                {
                    Total = total,
                    Articles = list
                };
            }
            
        }

        public async Task UpdateAsync(long id, UpdateArticleRequest data)
        {
            using (var db = new RonWebDbContext())
            {
                var sanitizer = new HtmlSanitizer();
                // 自定義規則
                sanitizer.AllowedSchemes.Add("mailto"); // 添加對 連結 屬性的支持
                sanitizer.AllowedAttributes.Add("class");
                sanitizer.AllowedAttributes.Add("alt"); // 添加對 alt 屬性的支持
                var article = await db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
                if (article != null)
                {
                    using (var tc = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            article.ArticleTitle = data.ArticleTitle;
                            article.Content = sanitizer.Sanitize(data.Content);
                            article.PreviewContent = sanitizer.Sanitize(data.PreviewContent);
                            article.CategoryId = data.CategoryId;
                            article.UpdateBy = data.UserId;
                            article.UpdateDate = DateTime.Now;
                            var mapping = await db.ArticleLabelMapping.Where(a => a.ArticleId == article.ArticleId).ToListAsync();
                            db.ArticleLabelMapping.RemoveRange(mapping);
                            var labelMapping = data.Labels.Select(a => new ArticleLabelMapping()
                            {
                                LabelId = a.LabelId,
                                ArticleId = article.ArticleId,
                                CreateDate = DateTime.Now,
                                CreateBy = data.UserId
                            }).ToList();
                            await db.ArticleLabelMapping.AddRangeAsync(labelMapping);
                            await db.SaveChangesAsync();
                            await tc.CommitAsync();
                        }
                        catch
                        {
                            await tc.RollbackAsync();
                            throw;
                        }
                    }

                }
                else
                {
                    throw new NotFoundException();
                }
            }
        }

        public async Task DeleteAsync(long id)
        {
            using (var db = new RonWebDbContext())
            {
                var article = await db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
                if (article != null)
                {
                    using (var tc = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            db.Article.Remove(article);
                            var list = await db.ArticleLabelMapping.Where(a => a.ArticleId == id).ToListAsync();
                            db.ArticleLabelMapping.RemoveRange(list);
                            await db.SaveChangesAsync();
                            await tc.CommitAsync();
                        }
                        catch
                        {
                            await tc.RollbackAsync();
                            throw;
                        }
                    }
                }
                else
                {
                    throw new NotFoundException();
                }
            }
        }

        public async Task CreateAsync(CreateArticleRequest data)
        {
            using (var db = new RonWebDbContext())
            {
                using (var tc = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var sanitizer = new HtmlSanitizer();
                        sanitizer.AllowedSchemes.Add("mailto"); // 添加對 連結 屬性的支持連結
                        sanitizer.AllowedAttributes.Add("class");
                        sanitizer.AllowedAttributes.Add("alt"); // 添加對 alt 屬性的支持
                        var article = new Article()
                        {
                            ArticleTitle = data.ArticleTitle,
                            Content = sanitizer.Sanitize(data.Content),
                            PreviewContent = sanitizer.Sanitize(data.PreviewContent),
                            CategoryId = data.CategoryId,
                            ViewCount = 0,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        };
                        await db.Article.AddAsync(article);
                        var mapping = data.Labels.Select(a => new ArticleLabelMapping()
                        {
                            ArticleId = article.ArticleId,
                            LabelId = a.LabelId,
                            CreateBy = data.UserId,
                            CreateDate = DateTime.Now
                        });
                        await db.ArticleLabelMapping.AddRangeAsync(mapping);
                        await db.SaveChangesAsync();
                        await tc.CommitAsync();
                    }
                    catch
                    {
                        await tc.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task UpdateArticleViews(long id)
        {
            using (var db = new RonWebDbContext())
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
}