using System;
using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.SiteMap;
using RonWeb.API.Models.SiteMap;
using RonWeb.Database.MySql.RonWeb.DataBase;

namespace RonWeb.API.Helper.SiteMap
{
	public class SiteMapHelper: ISiteMapHelper
	{
        public async Task<List<SiteMapResponse<long>>> Article()
        {
            using (var db = new RonWebDbContext())
            {
                var result = await db.Article.Select(a => new SiteMapResponse<long>()
                {
                    ID = a.ArticleId
                }).ToListAsync();
                return result;
            }
        }

        public async Task<List<SiteMapResponse<long>>> Category()
        {
            using (var db = new RonWebDbContext())
            {
                var result = await db.ArticleCategory.Select(a => new SiteMapResponse<long>()
                {
                    ID = a.CategoryId
                }).ToListAsync();
                return result;
            }
        }

        public async Task<List<SiteMapResponse<long>>> Label()
        {
            using (var db = new RonWebDbContext())
            {
                var result = await db.ArticleLabel.Select(a => new SiteMapResponse<long>()
                {
                    ID = a.LabelId
                }).ToListAsync();
                return result;
            }
        }
    }
}

