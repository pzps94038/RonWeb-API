using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Interface.AdminArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.AdminArticleCategory
{
    public class AdminArticleCategoryHelper : IAdminArticleCategoryHelper
    {
        private readonly RonWebDbContext _db;

        public AdminArticleCategoryHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<Category> GetAsync(long id)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category != null)
            {
                return new Category()
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    CreateDate = category.CreateDate
                };
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task CreateAsync(CreateArticleCategoryRequest data)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryName == data.CategoryName);
            if (category != null)
            {
                throw new UniqueException();
            }
            else
            {
                category = new Database.Entities.ArticleCategory()
                {
                    CategoryName = data.CategoryName,
                    CreateDate = DateTime.Now,
                    CreateBy = data.UserId
                };
                await _db.AddAsync(category);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(long id, UpdateArticleCategoryRequest data)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category != null)
            {
                category.CategoryName = data.CategoryName;
                category.UpdateBy = data.UserId;
                category.UpdateDate = DateTime.Now;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category != null)
            {
                var executionStrategy = _db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await _db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var articles = await _db.Article.Where(a => a.CategoryId == id).ToListAsync();
                            _db.Article.RemoveRange(articles);
                            _db.ArticleCategory.Remove(category);
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

        public async Task<GetArticleCategoryResponse> GetListAsync(int? page)
        {
            var query = _db.ArticleCategory.AsQueryable();
            var total = query.Count();
            if (page != null)
            {
                var pageSize = 10;
                int skip = (int)((page - 1) * pageSize);
                if (skip == 0)
                {
                    query = query.Take(pageSize);
                }
                else
                {
                    query = query.Skip(skip).Take(pageSize);
                }
            }
            var categorys = await query.Select(a => new Category()
            {
                CategoryId = a.CategoryId,
                CategoryName = a.CategoryName,
                CreateDate = a.CreateDate
            }).ToListAsync();
            return new GetArticleCategoryResponse()
            {
                Total = total,
                Categorys = categorys
            };
        }
    }
}
