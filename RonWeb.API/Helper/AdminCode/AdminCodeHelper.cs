using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CodeType;
using RonWeb.API.Models.CustomizeException;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.AdminArticleLabel
{
    public class AdminCodeHelper : IAdminCodeHelper
    {
        private readonly RonWebDbContext _db;

        public AdminCodeHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task CreateAsync(CreateCodeRequest data)
        {
            var code = await _db.Code.SingleOrDefaultAsync(a => a.CodeTypeId == a.CodeTypeId && a.CodeId == a.CodeId);
            if (code != null)
            {
                throw new UniqueException();
            }
            else
            {
                code = new Code()
                {
                    CodeTypeId = data.CodeTypeId,
                    CodeId = data.CodeId,
                    CodeName = data.CodeName,
                    CreateDate = DateTime.Now,
                    CreateBy = data.UserId
                };

                await _db.Code.AddAsync(code);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var code = await _db.Code.SingleOrDefaultAsync(a => a.Id == id);
            if (code != null)
            {
                _db.Code.RemoveRange(code);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<Code> GetAsync(long id)
        {
            var code = await _db.Code.SingleOrDefaultAsync(a => a.Id == id);
            if (code != null)
            {
                return code;
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<GetCodeResponse> GetListAsync(string codeTypeId, int? page)
        {
            var query = _db.Code.Where(a => a.CodeTypeId == a.CodeTypeId).AsQueryable();
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
            var codes = await query.Select(a => new Code()
            {
                Id = a.Id,
                CodeTypeId = a.CodeTypeId,
                CodeName = a.CodeName,
                CreateBy = a.CreateBy,
                CreateDate = a.CreateDate,
                UpdateBy = a.UpdateBy,
                UpdateDate = a.UpdateDate,
            }).ToListAsync();
            return new GetCodeResponse()
            {
                Total = total,
                Codes = codes
            };
        }

        public async Task UpdateAsync(long id, UpdateCodeRequest data)
        {
            var code = await _db.Code.SingleOrDefaultAsync(a => a.Id == id);
            if (code != null)
            {
                code.CodeId = data.CodeId;
                code.CodeName = data.CodeName;
                code.UpdateBy = data.UserId;
                code.UpdateDate = DateTime.Now;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}

