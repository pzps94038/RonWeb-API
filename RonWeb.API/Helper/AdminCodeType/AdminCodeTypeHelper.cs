using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CodeType;
using RonWeb.API.Models.CustomizeException;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.AdminArticleLabel
{
    public class AdminCodeTypeHelper : IAdminCodeTypeHelper
    {
        private readonly RonWebDbContext _db;

        public AdminCodeTypeHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task CreateAsync(CreateCodeTypeRequest data)
        {
            var codeType = await _db.CodeType.SingleOrDefaultAsync(a => a.CodeTypeId == data.CodeTypeId);
            if (codeType != null)
            {
                throw new UniqueException();
            }
            else
            {
                codeType = new CodeType()
                {
                    CodeTypeId = data.CodeTypeId,
                    CodeTypeName = data.CodeTypeName,
                    CreateDate = DateTime.Now,
                    CreateBy = data.UserId
                };

                await _db.AddAsync(codeType);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var codeType = await _db.CodeType.SingleOrDefaultAsync(a => a.CodeTypeId == id);
            if (codeType != null)
            {
                var executionStrategy = _db.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var tc = await _db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var mapping = await _db.Code.Where(a => a.CodeTypeId == id).ToListAsync();
                            if (mapping.Any())
                            {
                                _db.Code.RemoveRange(mapping);
                                _db.CodeType.Remove(codeType);
                            }
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

        public async Task<CodeType> GetAsync(string id)
        {
            var codeType = await _db.CodeType.SingleOrDefaultAsync(a => a.CodeTypeId == id);
            if (codeType != null)
            {
                return codeType;
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<GetCodeTypeResponse> GetListAsync(int? page)
        {
            var query = _db.CodeType.AsQueryable();
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
            var codeTypes = await query.Select(a => new CodeType()
            {
                CodeTypeId = a.CodeTypeId,
                CodeTypeName = a.CodeTypeName,
                CreateBy = a.CreateBy,
                CreateDate = a.CreateDate,
                UpdateBy = a.UpdateBy,
                UpdateDate = a.UpdateDate,
            }).ToListAsync();
            return new GetCodeTypeResponse()
            {
                Total = total,
                CodeTypes = codeTypes
            };
        }

        public async Task UpdateAsync(string id, UpdateCodeTypeRequest data)
        {
            var codeType = await _db.CodeType.SingleOrDefaultAsync(a => a.CodeTypeId == id);
            if (codeType != null)
            {
                codeType.CodeTypeName = data.CodeTypeName;
                codeType.UpdateBy = data.UserId;
                codeType.UpdateDate = DateTime.Now;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}

