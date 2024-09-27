using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.SiteMap;
using RonWeb.API.Models.SiteMap;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.SiteMap
{
    public class SiteMapHelper : ISiteMapHelper
    {
        private readonly RonWebDbContext _db;

        public SiteMapHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<List<SiteMapResponse<long>>> Article()
        {
            var result = await _db.Article.Where(a => a.Flag == "Y")
                .Select(a => new SiteMapResponse<long>()
                {
                    ID = a.ArticleId
                }).ToListAsync();
            return result;
        }

        public async Task<List<SiteMapResponse<long>>> Category()
        {
            var result = await _db.ArticleCategory.Select(a => new SiteMapResponse<long>()
            {
                ID = a.CategoryId
            })
            .ToListAsync();
            return result;
        }

        public async Task<List<SiteMapResponse<long>>> Label()
        {
            var result = await _db.ArticleLabel.Select(a => new SiteMapResponse<long>()
            {
                ID = a.LabelId
            })
            .ToListAsync();
            return result;
        }
    }
}

