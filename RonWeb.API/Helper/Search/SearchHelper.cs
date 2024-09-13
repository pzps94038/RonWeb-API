using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.Shared;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using RonWeb.API.Models.CustomizeException;
using RonWeb.Database.MySql.RonWeb.DataBase;
using Microsoft.EntityFrameworkCore;
using RonWeb.Database.Redis;
using Newtonsoft.Json;

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
                var query = _db.Article.Where(a => a.Flag == "Y" && a.CategoryId == id)
                .Include(a => a.ArticleCategory)
                .Include(a => a.ArticleLabelMapping)
                .ThenInclude(a => a.ArticleLabel)
                .Select(a => new ArticleItem()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    CategoryId = a.CategoryId,
                    CategoryName = a.ArticleCategory.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                    Labels = a.ArticleLabelMapping
                        .Select(mapping => new Label
                        {
                            LabelId = mapping.ArticleLabel.LabelId,
                            LabelName = mapping.ArticleLabel.LabelName
                        })
                        .ToList()
                })
                .OrderByDescending(a => a.CreateDate);

                var pageSize = 10;
                var skip = (curPage - 1) * pageSize;
                var total = query.Count();
                List<ArticleItem> list;
                if (skip == 0)
                {
                    list = await query.Take(pageSize).ToListAsync();
                }
                else
                {
                    list = await query.Skip(skip).Take(pageSize).ToListAsync();
                }
                var data = new KeywordeResponse()
                {
                    Total = total,
                    Articles = list,
                    Keyword = category.CategoryName
                };
                var json = JsonConvert.SerializeObject(data);
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
                var query = _db.Article.Where(a => a.Flag == "Y")
                .Include(a => a.ArticleCategory)
                .Include(a => a.ArticleLabelMapping)
                .ThenInclude(a => a.ArticleLabel)
                .Where(a => a.ArticleLabelMapping.Any(a => a.LabelId == id))
                .Select(a => new ArticleItem()
                {
                    ArticleId = a.ArticleId,
                    ArticleTitle = a.ArticleTitle,
                    PreviewContent = a.PreviewContent,
                    CategoryId = a.CategoryId,
                    CategoryName = a.ArticleCategory.CategoryName,
                    ViewCount = a.ViewCount,
                    CreateDate = a.CreateDate,
                    Labels = a.ArticleLabelMapping
                        .Select(mapping => new Label
                        {
                            LabelId = mapping.ArticleLabel.LabelId,
                            LabelName = mapping.ArticleLabel.LabelName
                        })
                        .ToList()
                })
                .OrderByDescending(a => a.CreateDate);
                var pageSize = 10;
                var skip = (curPage - 1) * pageSize;
                var total = query.Count();
                List<ArticleItem> list;
                if (skip == 0)
                {
                    list = await query.Take(pageSize).ToListAsync();
                }
                else
                {
                    list = await query.Skip(skip).Take(pageSize).ToListAsync();
                }
                var data = new KeywordeResponse()
                {
                    Total = total,
                    Articles = list,
                    Keyword = label.LabelName
                };
                var json = JsonConvert.SerializeObject(data);
                return data;
            }
        }
    }
}

