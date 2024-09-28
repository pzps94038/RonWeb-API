using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Interface.AdminArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
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
            if (category == null)
            {
                throw new NotFoundException();
            }
            return new Category()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CreateDate = category.CreateDate
            };
        }

        public async Task CreateAsync(CreateArticleCategoryRequest data)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryName == data.CategoryName);
            if (category != null)
            {
                throw new UniqueException();
            }
            category = new Database.Entities.ArticleCategory()
            {
                CategoryName = data.CategoryName,
                CreateDate = DateTime.Now,
                CreateBy = data.UserId
            };
            await _db.AddAsync(category);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(long id, UpdateArticleCategoryRequest data)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category == null)
            {
                throw new NotFoundException();
            }
            category.CategoryName = data.CategoryName;
            category.UpdateBy = data.UserId;
            category.UpdateDate = DateTime.Now;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category == null)
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

        public async Task<GetArticleCategoryResponse> GetListAsync(int? page)
        {
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var query = _db.ArticleCategory.AsQueryable();
            var total = await query.CountAsync();
            var categorys = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(a => new Category()
                {
                    CategoryId = a.CategoryId,
                    CategoryName = a.CategoryName,
                    CreateDate = a.CreateDate
                })
                .ToListAsync();
            return new GetArticleCategoryResponse()
            {
                Total = total,
                Categorys = categorys
            };
        }
    }
}
