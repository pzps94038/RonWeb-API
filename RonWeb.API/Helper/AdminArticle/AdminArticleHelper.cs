using System.IO;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using RonWeb.API.Enum;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
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
            var vwArticleList = await _db.VwArticle.Where(a => a.ArticleId == id).ToListAsync();
            if (vwArticleList.Any())
            {
                var article = vwArticleList.First();
                var data = vwArticleList
                    .GroupBy(a => a.ArticleId)
                    .Select(a => new GetByIdArticleResponse()
                    {
                        ArticleId = article.ArticleId,
                        ArticleTitle = article.ArticleTitle,
                        PreviewContent = article.PreviewContent,
                        Content = article.Content,
                        CategoryId = article.CategoryId,
                        CategoryName = article.CategoryName ?? "",
                        Flag = article.Flag,
                        ViewCount = article.ViewCount,
                        CreateDate = article.ArticleCreateDate,
                        Labels = a.Select(article => new Models.Shared.Label()
                        {
                            LabelId = article.LabelId,
                            LabelName = article.LabelName,
                            CreateDate = article.LabelCreateDate
                        })
                            .OrderBy(label => label.CreateDate)
                            .GroupBy(label => label.LabelId)
                            .Select(g => g.First())
                            .ToList(),
                        References = a.Where(a => a.Link != null)
                            .Select(a => a.Link!)
                            .OrderBy(a => a)
                            .GroupBy(a => a)
                            .Select(g => g.First())
                            .ToList(),
                    })
                    .First();
                // 找到下一篇文章
                var nextArticle = await _db.Article.Where(a => a.CreateDate > data.CreateDate)
                    .OrderBy(a => a.CreateDate)
                    .Take(1)
                    .Select(a => new BlogPagination()
                    {
                        ArticleId = a.ArticleId,
                        ArticleTitle = a.ArticleTitle
                    }).FirstOrDefaultAsync();
                // 找到上一篇文章
                var prevArticle = await _db.Article.Where(a => a.CreateDate < data.CreateDate)
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
             .OrderByDescending(a => a.CreateDate)
             .ToList();
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
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                var executionStrategy = _db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await _db.Database.BeginTransactionAsync())
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
                            var mapping = await _db.ArticleLabelMapping.Where(a => a.ArticleId == article.ArticleId).ToListAsync();
                            _db.ArticleLabelMapping.RemoveRange(mapping);
                            var labelMapping = data.Labels.Select(a => new ArticleLabelMapping()
                            {
                                LabelId = (long)a.LabelId!,
                                ArticleId = article.ArticleId,
                                CreateDate = DateTime.Now,
                                CreateBy = data.UserId
                            }).ToList();
                            await _db.ArticleLabelMapping.AddRangeAsync(labelMapping);

                            var prevFiles = data.PrevFiles.Select(a => new ArticlePrevImage()
                            {
                                ArticleId = article.ArticleId,
                                FileName = a.FileName,
                                Path = a.Path,
                                Url = a.Url,
                                CreateDate = DateTime.Now,
                                CreateBy = data.UserId
                            }).ToList();

                            _db.ArticlePrevImage.AddRange(prevFiles);

                            var files = data.ContentFiles.Select(a => new ArticleImage()
                            {
                                ArticleId = article.ArticleId,
                                FileName = a.FileName,
                                Path = a.Path,
                                Url = a.Url,
                                CreateDate = DateTime.Now,
                                CreateBy = data.UserId
                            }).ToList();

                            _db.ArticleImage.AddRange(files);
                            var existReferences = await _db.ArticleReferences.Where(a => a.ArticleId == article.ArticleId).ToListAsync();
                            if (existReferences.Any())
                            {
                                _db.ArticleReferences.RemoveRange(existReferences);
                            }
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
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                var executionStrategy = _db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await _db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            _db.Article.Remove(article);
                            var labels = await _db.ArticleLabelMapping.Where(a => a.ArticleId == id).ToListAsync();
                            _db.ArticleLabelMapping.RemoveRange(labels);

                            var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                            var storageTool = new FireBaseStorageTool(storageBucket);

                            var prevImages = await _db.ArticlePrevImage.Where(a => a.ArticleId == id).ToListAsync();
                            _db.ArticlePrevImage.RemoveRange(prevImages);
                            var prevImgTasks = prevImages.Select(a => Task.Run(async () =>
                            {
                                await storageTool.Delete(a.Path);
                            })).ToArray();
                            Task.WaitAll(prevImgTasks);

                            var images = await _db.ArticleImage.Where(a => a.ArticleId == id).ToListAsync();
                            _db.ArticleImage.RemoveRange(images);
                            var imgTasks = images.Select(a => Task.Run(async () =>
                            {
                                await storageTool.Delete(a.Path);
                            })).ToArray();
                            Task.WaitAll(imgTasks);
                            var existReferences = await _db.ArticleReferences.Where(a => a.ArticleId == id).ToListAsync();
                            if (existReferences.Any())
                            {
                                _db.ArticleReferences.RemoveRange(existReferences);
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
            else
            {
                throw new NotFoundException();
            }
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

                        var mappings = data.Labels.Select(a => new ArticleLabelMapping()
                        {
                            ArticleId = article.ArticleId,
                            LabelId = (long)a.LabelId!,
                            CreateBy = data.UserId,
                            CreateDate = DateTime.Now
                        });

                        _db.ArticleLabelMapping.AddRange(mappings);

                        var prevFiles = data.PrevFiles.Select(a => new ArticlePrevImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();

                        _db.ArticlePrevImage.AddRange(prevFiles);

                        var files = data.PrevFiles.Select(a => new ArticleImage()
                        {
                            ArticleId = article.ArticleId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        _db.ArticleImage.AddRange(files);

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

    }
}

