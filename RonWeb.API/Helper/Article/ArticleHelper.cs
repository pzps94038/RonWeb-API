using MongoDB.Driver;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RonWeb.Database.Entities;
using System.Linq;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Interface.AdminArticleHelper;

namespace RonWeb.API.Helper
{
    public class ArticleHelper : IArticleHelper
    {
        private readonly IAdminArticleHelper _adminArticleHelper;
        private readonly RonWebDbContext _db;

        public ArticleHelper(
            RonWebDbContext dbContext,
            IAdminArticleHelper adminArticleHelper
        )
        {
            _db = dbContext;
            _adminArticleHelper = adminArticleHelper;
        }

        public async Task<GetByIdArticleResponse> GetAsync(long id)
        {
            return await _adminArticleHelper.GetAsync(id);
        }

        public async Task<GetArticleResponse> GetListAsync(int? page, string? keyword)
        {
            return await _adminArticleHelper.GetListAsync(page, keyword);
        }

        public async Task UpdateArticleViews(long id)
        {
            var article = await _db.Article.SingleOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
            {
                throw new NotFoundException();
            }
            article.ViewCount = article.ViewCount + 1;
            await _db.SaveChangesAsync();
        }
    }
}