using System;
using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Database.MySql.RonWeb.DataBase;

namespace RonWeb.API.Helper.ArticleLabel
{
    public class ArticleLabelHelper : IArticleLabelHelper
    {
        public async Task CreateAsync(CreateArticleLabelRequest data)
        {
            using (var db = new RonWebDbContext())
            {
                var label = await db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelName == data.LabelName);
                if (label != null)
                {
                    throw new UniqueException();
                }
                else
                {
                    label = new RonWeb.Database.MySql.RonWeb.Table.ArticleLabel()
                    {
                        LabelName = data.LabelName,
                        CreateDate = DateTime.Now,
                        CreateBy = data.UserId
                    };

                    await db.AddAsync(label);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(long id)
        {
            using (var db = new RonWebDbContext())
            {
                var label = await db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
                if (label != null)
                {
                    using (var tc = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var mapping = await db.ArticleLabelMapping.Where(a => a.LabelId == id).ToListAsync();
                            db.ArticleLabelMapping.RemoveRange(mapping);
                            db.ArticleLabel.Remove(label);
                            await db.SaveChangesAsync();
                            await tc.CommitAsync();
                        }
                        catch
                        {
                            await tc.RollbackAsync();
                            throw;
                        }
                    }
                }
                else
                {
                    throw new NotFoundException();
                }
            }
        }

        public async Task<Label> GetAsync(long id)
        {
            using (var db = new RonWebDbContext())
            {
                var label = await db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
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
        }

        public async Task<GetArticleLabelResponse> GetListAsync(int? page)
        {
            using (var db = new RonWebDbContext())
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
                return new GetArticleLabelResponse()
                {
                    Total = total,
                    Labels = labels
                };
            }
        }

        public async Task UpdateAsync(long id, UpdateArticleLabelRequest data)
        {
            using (var db = new RonWebDbContext())
            {
                var label = await db.ArticleLabel.SingleOrDefaultAsync(a => a.LabelId == id);
                if (label != null)
                {
                    label.LabelName = data.LabelName;
                    label.UpdateBy = data.UserId;
                    label.UpdateDate = DateTime.Now;
                    await db.SaveChangesAsync();
                }
                else
                {
                    throw new NotFoundException();
                }
            }
        }
    }
}

