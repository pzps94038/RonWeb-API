using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.AdminArticleLabel;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.AdminArticleLabel
{
    public class AdminArticleLabelHelper : IAdminArticleLabelHelper
    {
        private readonly RonWebDbContext _db;

        public AdminArticleLabelHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task CreateAsync(CreateArticleLabelRequest data)
        {
            var label = await _db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelName == data.LabelName);
            if (label != null)
            {
                throw new UniqueException();
            }
            else
            {
                label = new Database.Entities.ArticleLabel()
                {
                    LabelName = data.LabelName,
                    CreateDate = DateTime.Now,
                    CreateBy = data.UserId
                };

                await _db.AddAsync(label);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var label = await _db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
            if (label != null)
            {
                var executionStrategy = _db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await _db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var mapping = await _db.ArticleLabelMapping.Where(a => a.LabelId == id).ToListAsync();
                            _db.ArticleLabelMapping.RemoveRange(mapping);
                            _db.ArticleLabel.Remove(label);
                            await _db.SaveChangesAsync();
                            await tc.CommitAsync();
                        }
                        catch
                        {
                            await tc.RollbackAsync();
                            throw;
                        }
                    }
                });
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<Label> GetAsync(long id)
        {
            var label = await _db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
            if (label != null)
            {
                return new Label()
                {
                    LabelId = label.LabelId,
                    LabelName = label.LabelName,
                    CreateDate = label.CreateDate
                };
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<GetArticleLabelResponse> GetListAsync(int? page)
        {
            var query = _db.ArticleLabel.AsQueryable();
            var total = query.Count();
            if (page != null)
            {
                var pageSize = 10;
                int skip = (int)((page - 1) * pageSize);
                query = skip == 0 ? query.Take(pageSize) : query.Skip(skip).Take(pageSize);
            }
            var labels = await query.Select(a => new Label()
            {
                LabelId = a.LabelId,
                LabelName = a.LabelName,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
            return new GetArticleLabelResponse()
            {
                Total = total,
                Labels = labels
            };
        }

        public async Task UpdateAsync(long id, UpdateArticleLabelRequest data)
        {
            var label = await _db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
            if (label != null)
            {
                label.LabelName = data.LabelName;
                label.UpdateBy = data.UserId;
                label.UpdateDate = DateTime.Now;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}

