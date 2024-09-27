using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.Shared;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using RonWeb.API.Models.CustomizeException;
using Microsoft.EntityFrameworkCore;
using RonWeb.Database.Redis;
using Newtonsoft.Json;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.Search
{
    public class SearchHelper : ISearchHelper
    {
        private readonly RonWebDbContext _db;

        public SearchHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<KeywordeResponse> Category(long id, int? page)
        {
            var curPage = page.GetValueOrDefault(1);
            var category = await _db.ArticleCategory.SingleOrDefaultAsync(a => a.CategoryId == id);
            if (category == null)
            {
                throw new NotFoundException();
            }
            else
            {
                var query = _db.VwArticle.Where(a => a.CategoryId == id && a.Flag == "Y");
                var idQuery = query.Select(a => new { a.ArticleId, a.ArticleCreateDate })
                    .Distinct()
                    .OrderByDescending(a => a.ArticleCreateDate)
                    .AsQueryable();
                var pageSize = 10;
                var skip = (curPage - 1) * pageSize;
                idQuery = skip == 0 ? idQuery.Take(pageSize) : idQuery.Skip(skip).Take(pageSize);
                var idList = idQuery.Select(a => a.ArticleId).ToList();
                var group = query.GroupBy(a => a.ArticleId);
                var total = group.Count();
                List<ArticleItem> list = (await _db.VwArticle.Where(a => idList.Contains(a.ArticleId)).ToListAsync())
                    .GroupBy(a => a.ArticleId)
                    .Select(a => new ArticleItem()
                    {
                        ArticleId = a.Key,
                        ArticleTitle = a.First().ArticleTitle,
                        PreviewContent = a.First().PreviewContent,
                        Content = a.First().Content,
                        CategoryId = a.First().CategoryId,
                        CategoryName = a.First().CategoryName ?? "",
                        ViewCount = a.First().ViewCount,
                        Flag = a.First().Flag,
                        CreateDate = a.First().ArticleCreateDate,
                        Labels = a.Select(article => new Models.Shared.Label()
                        {
                            LabelId = article.LabelId,
                            LabelName = article.LabelName,
                            CreateDate = article.LabelCreateDate
                        })
                        .OrderBy(label => label.CreateDate)
                        .GroupBy(label => label.LabelId)
                        .Select(g => g.First())
                        .ToList()
                    })
                    .OrderByDescending(a => a.CreateDate)
                    .ToList();
                var data = new KeywordeResponse()
                {
                    Total = total,
                    Articles = list,
                    Keyword = category.CategoryName
                };
                return data;
            }
        }

        public async Task<KeywordeResponse> Label(long id, int? page)
        {
            var curPage = page.GetValueOrDefault(1);
            var label = await _db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
            if (label == null)
            {
                throw new NotFoundException();
            }
            else
            {
                var query = _db.VwArticle.Where(a => a.CategoryId == id && a.Flag == "Y");
                var idQuery = query.Select(a => new { a.ArticleId, a.ArticleCreateDate })
                    .Distinct()
                    .OrderByDescending(a => a.ArticleCreateDate)
                    .AsQueryable();
                var pageSize = 10;
                var skip = (curPage - 1) * pageSize;
                idQuery = skip == 0 ? idQuery.Take(pageSize) : idQuery.Skip(skip).Take(pageSize);
                var idList = idQuery.Select(a => a.ArticleId).ToList();
                var group = query.GroupBy(a => a.ArticleId);
                var total = group.Count();
                List<ArticleItem> list = (await _db.VwArticle.Where(a => idList.Contains(a.ArticleId)).ToListAsync())
                    .GroupBy(a => a.ArticleId)
                    .Select(a => new ArticleItem()
                    {
                        ArticleId = a.Key,
                        ArticleTitle = a.First().ArticleTitle,
                        PreviewContent = a.First().PreviewContent,
                        Content = a.First().Content,
                        CategoryId = a.First().CategoryId,
                        CategoryName = a.First().CategoryName ?? "",
                        ViewCount = a.First().ViewCount,
                        Flag = a.First().Flag,
                        CreateDate = a.First().ArticleCreateDate,
                        Labels = a.Select(article => new Models.Shared.Label()
                        {
                            LabelId = article.LabelId,
                            LabelName = article.LabelName,
                            CreateDate = article.LabelCreateDate
                        })
                        .OrderBy(label => label.CreateDate)
                        .GroupBy(label => label.LabelId)
                        .Select(g => g.First())
                        .ToList()
                    })
                    .OrderByDescending(a => a.CreateDate)
                    .ToList();
                var data = new KeywordeResponse()
                {
                    Total = total,
                    Articles = list,
                    Keyword = label.LabelName
                };
                return data;
            }
        }
    }
}

