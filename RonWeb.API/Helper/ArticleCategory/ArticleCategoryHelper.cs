using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.Shared;
using RonWeb.Database.MySql.RonWeb.DataBase;

namespace RonWeb.API.Helper.ArticleCategory
{
    public class ArticleCategoryHelper : IArticleCategoryHelper
    {
        private readonly RonWebDbContext _db;

        public ArticleCategoryHelper(RonWebDbContext dbContext)
        {
            this._db = dbContext;
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
