using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Database.MySql.RonWeb.DataBase;
using RonWeb.Database.Redis;

namespace RonWeb.API.Helper.ArticleCategory
{
    public class ArticleCategoryHelper : IArticleCategoryHelper
    {
        private readonly RonWebDbContext db;
        private readonly Task<RedisConnection> _redisConnectionFactory;
        private readonly TimeSpan _cacheSpan = TimeSpan.FromHours(1);

        public ArticleCategoryHelper(RonWebDbContext dbContext, Task<RedisConnection> redisConnectionFactory)
        {
            this.db = dbContext;
            this._redisConnectionFactory = redisConnectionFactory;
        }

        public async Task<GetArticleCategoryResponse> GetListAsync(int? page)
        {
            var redisConection = await this._redisConnectionFactory;
            var key = RedisKeys.ArticleCategoryPrefix;
            var cache = await redisConection.BasicRetryAsync(async (db) => await db.StringGetAsync(key));
            if (cache.HasValue)
            {
                return JsonConvert.DeserializeObject<GetArticleCategoryResponse>(cache.ToString())!;
            }
            else
            {
                var query = db.ArticleCategory.AsQueryable();
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
                await redisConection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, json, _cacheSpan));
                return data;
            }
            
        }
    }
}
