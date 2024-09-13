using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Database.MySql.RonWeb.DataBase;

namespace RonWeb.API.Helper.ArticleLabel
{
    public class ArticleLabelHelper : IArticleLabelHelper
    {
        private readonly RonWebDbContext _db;

        public ArticleLabelHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<GetArticleLabelResponse> GetListAsync(int? page)
        {
            var query = _db.ArticleLabel.AsQueryable();
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
            return data;
        }
    }
}

