using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.ArticleLabel
{
    public class ArticleLabelHelper : IArticleLabelHelper
    {
        private readonly RonWebDbContext _db;

        public ArticleLabelHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<GetArticleLabelResponse> GetListAsync()
        {
            var query = _db.ArticleLabel.AsQueryable();
            var total = await query.CountAsync();
            var labels = await query.Select(a => new Label()
            {
                LabelId = a.LabelId,
                LabelName = a.LabelName,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
            var data = new GetArticleLabelResponse()
            {
                Total = total,
                Labels = labels
            };
            return data;
        }
    }
}

