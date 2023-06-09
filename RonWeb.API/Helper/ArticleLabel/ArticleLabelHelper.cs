using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Database.MySql.RonWeb.DataBase;
using RonWeb.Database.Redis;

namespace RonWeb.API.Helper.ArticleLabel
{
    public class ArticleLabelHelper : IArticleLabelHelper
    {
        private readonly RonWebDbContext db;
        private readonly Task<RedisConnection> _redisConnectionFactory;
        private readonly TimeSpan _cacheSpan = TimeSpan.FromHours(1);

        public ArticleLabelHelper(RonWebDbContext dbContext, Task<RedisConnection> redisConnectionFactory)
        {
            this.db = dbContext;
            this._redisConnectionFactory = redisConnectionFactory;
        }

        public async Task<GetArticleLabelResponse> GetListAsync(int? page)
        {
            var redisConection = await this._redisConnectionFactory;
            var key = RedisKeys.ArticleLabelPrefix;
            var cache = await redisConection.BasicRetryAsync(async (db) => await db.StringGetAsync(key));
            if (cache.HasValue)
            {
                return JsonConvert.DeserializeObject<GetArticleLabelResponse>(cache.ToString())!;
            }
            else
            {
                var query = db.ArticleLabel.AsQueryable();
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
                var labels = await query.Select(a => new Label()
                {
                    LabelId = a.LabelId,
                    LabelName = a.LabelName,
                    CreateDate = a.CreateDate
                }).ToListAsync();
                var data = new GetArticleLabelResponse()
                {
                    Total = total,
                    Labels = labels
                };
                var json = JsonConvert.SerializeObject(data);
                await redisConection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, json, _cacheSpan));
                return data;
            }
           
        }
    }
}

