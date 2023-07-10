using System.IO;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using RonWeb.API.Enum;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.MySql.RonWeb.DataBase;
using RonWeb.Database.MySql.RonWeb.Table;

namespace RonWeb.API.Helper.AdminArticle
{
    public class AdminArticleHelper : IAdminArticleHelper
    {
        private readonly RonWebDbContext db;

        public AdminArticleHelper(RonWebDbContext dbContext)
        {
            this.db = dbContext;
        }

        public async Task<GetByIdArticleResponse> GetAsync(long id)
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
                    }).SingleOrDefaultAsync(a => a.ArticleId == id);

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
                        Flag = a.Flag,
                        CreateDate = a.CreateDate,
                        Labels = a.ArticleLabelMapping
                            .Select(mapping => new Label
                            {
                                LabelId = mapping.ArticleLabel.LabelId,
                                LabelName = mapping.ArticleLabel.LabelName
                            })
                            .ToList()
                    });
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
            return data;
        }

        public async Task UpdateAsync(long id, UpdateArticleRequest data)
        {
            var sanitizer = new HtmlSanitizer();
            // 自定義規則
            sanitizer.AllowedSchemes.Add("mailto"); // 添加對 連結 屬性的支持
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("alt"); // 添加對 alt 屬性的支持
            var article = await db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                var executionStrategy = db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            article.ArticleTitle = data.ArticleTitle;
                            article.Content = sanitizer.Sanitize(data.Content);
                            article.PreviewContent = sanitizer.Sanitize(data.PreviewContent);
                            article.CategoryId = data.CategoryId;
                            article.Flag = data.Flag;
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

                            var prevFiles = data.PrevFiles.Select(a => new ArticlePrevImage()
                            {
                                ArticleId = article.ArticleId,
                                FileName = a.FileName,
                                Path = a.Path,
                                Url = a.Url,
                                CreateDate = DateTime.Now,
                                CreateBy = data.UserId
                            }).ToList();

                            db.ArticlePrevImage.AddRange(prevFiles);

                            var files = data.ContentFiles.Select(a => new ArticleImage()
                            {
                                ArticleId = article.ArticleId,
                                FileName = a.FileName,
                                Path = a.Path,
                                Url = a.Url,
                                CreateDate = DateTime.Now,
                                CreateBy = data.UserId
                            }).ToList();

                            db.ArticleImage.AddRange(files);

                            await db.SaveChangesAsync();
                            await tc.CommitAsync();
                        }
                        catch
                        {
                            await tc.RollbackAsync();
                            throw;
                        }
                    }
                });
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var article = await db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                var executionStrategy = db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            db.Article.Remove(article);
                            var labels = await db.ArticleLabelMapping.Where(a => a.ArticleId == id).ToListAsync();
                            db.ArticleLabelMapping.RemoveRange(labels);

                            var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                            var storageTool = new FireBaseStorageTool(storageBucket);

                            var prevImages = await db.ArticlePrevImage.Where(a => a.ArticleId == id).ToListAsync();
                            db.ArticlePrevImage.RemoveRange(prevImages);
                            var prevImgTasks = prevImages.Select(a => Task.Run(async () => {
                                await storageTool.Delete(a.Path);
                            })).ToArray();
                            Task.WaitAll(prevImgTasks);

                            var images = await db.ArticleImage.Where(a => a.ArticleId == id).ToListAsync();
                            db.ArticleImage.RemoveRange(images);
                            var imgTasks = images.Select(a => Task.Run(async () => {
                                await storageTool.Delete(a.Path);
                            })).ToArray();
                            Task.WaitAll(imgTasks);

                            await db.SaveChangesAsync();
                            await tc.CommitAsync();
                        }
                        catch
                        {
                            await tc.RollbackAsync();
                            throw;
                        }
                    }
                });
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task CreateAsync(CreateArticleRequest data)
        {
            var executionStrategy = db.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var sanitizer = new HtmlSanitizer();
                        sanitizer.AllowedSchemes.Add("mailto");
                        sanitizer.AllowedAttributes.Add("class");
                        sanitizer.AllowedAttributes.Add("alt");

                        var article = new Article()
                        {
                            ArticleTitle = data.ArticleTitle,
                            Content = sanitizer.Sanitize(data.Content),
                            PreviewContent = sanitizer.Sanitize(data.PreviewContent),
                            CategoryId = data.CategoryId,
                            Flag = data.Flag,
                            ViewCount = 0,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        };

                        db.Article.Add(article);
                        await db.SaveChangesAsync();

                        var mappings = data.Labels.Select(a => new ArticleLabelMapping()
                        {
                            ArticleId = article.ArticleId,
                            LabelId = a.LabelId,
                            CreateBy = data.UserId,
                            CreateDate = DateTime.Now
                        });

                        db.ArticleLabelMapping.AddRange(mappings);

                        var prevFiles = data.PrevFiles.Select(a => new ArticlePrevImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();

                        db.ArticlePrevImage.AddRange(prevFiles);

                        var files = data.PrevFiles.Select(a => new ArticleImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();

                        db.ArticleImage.AddRange(files);

                        await db.SaveChangesAsync();

                        await tc.CommitAsync();
                    }
                    catch
                    {
                        await tc.RollbackAsync();
                        throw;
                    }
                }
            });
        }

    }
}

