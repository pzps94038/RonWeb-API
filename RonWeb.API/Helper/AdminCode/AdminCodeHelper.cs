using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Code;
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
            var code = await _db.Code.SingleOrDefaultAsync(a => a.CodeTypeId == data.CodeTypeId && a.CodeId == data.CodeId);
            if (code != null)
            {
                throw new UniqueException();
            }
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

        public async Task DeleteAsync(long id)
        {
            var code = await _db.Code.SingleOrDefaultAsync(a => a.Id == id);
            if (code == null)
            {
                throw new NotFoundException();
            }
            _db.Code.RemoveRange(code);
            await _db.SaveChangesAsync();
        }

        public async Task<VwCode> GetAsync(long id)
        {
            var code = await _db.VwCode.SingleOrDefaultAsync(a => a.Id == id);
            if (code == null)
            {
                throw new NotFoundException();
            }
            return code;
        }

        public async Task<GetCodeResponse> GetListAsync(string codeTypeId, int? page)
        {
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;
            var codeType = await _db.CodeType.Where(a => a.CodeTypeId == codeTypeId).SingleOrDefaultAsync();
            if (codeType == null)
            {
                throw new NotFoundException();
            }
            var query = _db.Code.Where(a => a.CodeTypeId == codeType.CodeTypeId);
            var total = await query.CountAsync();
            var codes = await query.Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            return new GetCodeResponse()
            {
                Total = total,
                Codes = codes,
                CodeTypeId = codeType.CodeTypeId,
                CodeTypeName = codeType.CodeTypeName
            };
        }

        public async Task UpdateAsync(long id, UpdateCodeRequest data)
        {
            var code = await _db.Code.SingleOrDefaultAsync(a => a.Id == id);
            if (code == null)
            {
                throw new NotFoundException();
            }
            code.CodeId = data.CodeId;
            code.CodeName = data.CodeName;
            code.UpdateBy = data.UserId;
            code.UpdateDate = DateTime.Now;
            await _db.SaveChangesAsync();
        }
    }
}

