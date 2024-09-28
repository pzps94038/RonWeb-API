using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using RonWeb.API.Enum;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.Core;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.AdminArticle
{
    public class AdminArticleHelper : IAdminArticleHelper
    {
        private readonly RonWebDbContext _db;

        public AdminArticleHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<GetByIdArticleResponse> GetAsync(long id)
        {
            var article = await _db.Article.Where(a => a.ArticleId == id).SingleOrDefaultAsync();
            if (article == null)
            {
                throw new NotFoundException();
            }

            // 分類名稱
            var categoryName = (await _db.ArticleCategory.FirstOrDefaultAsync(a => a.CategoryId == article.CategoryId))?.CategoryName ?? "";

            // 文章標籤查詢
            var articleLabelQuery = _db.ArticleLabelMapping.Where(a => a.ArticleId == article.ArticleId);
            var articleLabels = await _db.ArticleLabel
                .Join(articleLabelQuery, a => a.LabelId, b => b.LabelId, (a, b) => new Label(a.LabelId, a.LabelName, b.CreateDate))
                .ToListAsync();

            // 參考文章
            var references = await _db.ArticleReferences
                .Where(a => a.ArticleId == article.ArticleId)
                .Select(a => a.Link)
                .ToListAsync();

            // 找到下一篇文章
            var nextArticle = await _db.Article.Where(a => a.CreateDate > article.CreateDate)
                .OrderBy(a => a.CreateDate)
                .Take(1)
                .Select(a => new BlogPagination()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle
                }).FirstOrDefaultAsync();
            // 找到上一篇文章
            var prevArticle = await _db.Article.Where(a => a.CreateDate < article.CreateDate)
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
                .Join(_db.ArticleCategory, a => a.CategoryId, b => b.CategoryId, (a, b) => new
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
            var articleList = (await query.ToListAsync())
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
                        .Select(a => new Label(a.LabelId, a.LabelName, a.CreateDate))
                        .ToList()
                })
                .ToList();

            return new GetArticleResponse()
            {
                Total = total,
                Articles = articleList
            };
        }

        public async Task UpdateAsync(long id, UpdateArticleRequest data)
        {
            var sanitizer = new HtmlSanitizer();
            // 自定義規則
            sanitizer.AllowedSchemes.Add("mailto"); // 添加對 連結 屬性的支持
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("alt"); // 添加對 alt 屬性的支持
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
            {
                throw new NotFoundException();
            }
            var executionStrategy = _db.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // 文章更新
                        article.ArticleTitle = data.ArticleTitle;
                        article.Content = sanitizer.Sanitize(data.Content);
                        article.PreviewContent = sanitizer.Sanitize(data.PreviewContent);
                        article.CategoryId = data.CategoryId;
                        article.Flag = data.Flag;
                        article.UpdateBy = data.UserId;
                        article.UpdateDate = DateTime.Now;

                        // 既有標籤移除
                        var existMapping = await _db.ArticleLabelMapping.Where(a => a.ArticleId == article.ArticleId).ToListAsync();
                        if (existMapping.Any())
                        {
                            _db.ArticleLabelMapping.RemoveRange(existMapping);
                        }

                        // 新標籤添加
                        var labelMapping = data.Labels.Select(a => new ArticleLabelMapping()
                        {
                            LabelId = (long)a.LabelId!,
                            ArticleId = article.ArticleId,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (labelMapping.Any())
                        {
                            await _db.ArticleLabelMapping.AddRangeAsync(labelMapping);
                        }

                        // 預覽上傳添加
                        var prevFiles = data.PrevFiles.Select(a => new ArticleImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (prevFiles.Any())
                        {
                            await _db.ArticleImage.AddRangeAsync(prevFiles);
                        }

                        // 內容上傳添加
                        var files = data.ContentFiles.Select(a => new ArticleImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (files.Any())
                        {
                            await _db.ArticleImage.AddRangeAsync(files);
                        }

                        // 既有參考文章移除
                        var existReferences = await _db.ArticleReferences.Where(a => a.ArticleId == article.ArticleId).ToListAsync();
                        if (existReferences.Any())
                        {
                            _db.ArticleReferences.RemoveRange(existReferences);
                        }

                        // 參考文章添加
                        var references = data.References.Select(a => new ArticleReferences
                        {
                            ArticleId = article.ArticleId,
                            Link = a,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (references.Any())
                        {
                            _db.ArticleReferences.AddRange(references);
                        }
                        await _db.SaveChangesAsync();
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

        public async Task DeleteAsync(long id)
        {
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
            {
                throw new NotFoundException();
            }
            var executionStrategy = _db.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // 移除文章
                        _db.Article.Remove(article);

                        // 移除文章對應標籤
                        var existLabels = await _db.ArticleLabelMapping.Where(a => a.ArticleId == id).ToListAsync();
                        if (existLabels.Any())
                        {
                            _db.ArticleLabelMapping.RemoveRange(existLabels);
                        }

                        // 移除圖片關聯
                        var images = await _db.ArticleImage.Where(a => a.ArticleId == id).ToListAsync();
                        if (images.Any())
                        {
                            _db.ArticleImage.RemoveRange(images);
                        }

                        // 移除參考文章
                        var existReferences = await _db.ArticleReferences.Where(a => a.ArticleId == id).ToListAsync();
                        if (existReferences.Any())
                        {
                            _db.ArticleReferences.RemoveRange(existReferences);
                        }
                        await _db.SaveChangesAsync();
                        await tc.CommitAsync();

                        // 移除雲端圖片
                        var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                        var storageTool = new FireBaseStorageTool(storageBucket);
                        foreach (var image in images)
                        {
                            await storageTool.Delete(image.Path);
                        }
                    }
                    catch
                    {
                        await tc.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        public async Task CreateAsync(CreateArticleRequest data)
        {
            var executionStrategy = _db.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var sanitizer = new HtmlSanitizer();
                        sanitizer.AllowedSchemes.Add("mailto");
                        sanitizer.AllowedAttributes.Add("class");
                        sanitizer.AllowedAttributes.Add("alt");
                        // 新增文章
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
                        _db.Article.Add(article);
                        await _db.SaveChangesAsync();

                        // 新增文章標籤關聯
                        var mappings = data.Labels.Select(a => new ArticleLabelMapping()
                        {
                            ArticleId = article.ArticleId,
                            LabelId = (long)a.LabelId!,
                            CreateBy = data.UserId,
                            CreateDate = DateTime.Now
                        });
                        if (mappings.Any())
                        {
                            await _db.ArticleLabelMapping.AddRangeAsync(mappings);
                        }

                        // 新增預覽圖
                        var prevFiles = data.PrevFiles.Select(a => new ArticleImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (prevFiles.Any())
                        {
                            await _db.ArticleImage.AddRangeAsync(prevFiles);
                        }

                        // 新增內容圖
                        var files = data.PrevFiles.Select(a => new ArticleImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (files.Any())
                        {
                            await _db.ArticleImage.AddRangeAsync(files);
                        }

                        // 新增參考文章
                        var references = data.References.Select(a => new ArticleReferences
                        {
                            ArticleId = article.ArticleId,
                            Link = a,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (references.Any())
                        {
                            await _db.ArticleReferences.AddRangeAsync(references);
                        }

                        await _db.SaveChangesAsync();
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

