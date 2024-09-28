using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.Shared;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.ArticleCategory
{
    public class ArticleCategoryHelper : IArticleCategoryHelper
    {
        private readonly RonWebDbContext _db;

        public ArticleCategoryHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<GetArticleCategoryResponse> GetListAsync()
        {
            var query = _db.ArticleCategory.AsQueryable();
            var total = await query.CountAsync();
            var categorys = await query.Select(a => new Category()
            {
                CategoryId = a.CategoryId,
                CategoryName = a.CategoryName,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
            var data = new GetArticleCategoryResponse()
            {
                Total = total,
                Categorys = categorys
            };
            var json = JsonConvert.SerializeObject(data);
            return data;
        }
    }
}
