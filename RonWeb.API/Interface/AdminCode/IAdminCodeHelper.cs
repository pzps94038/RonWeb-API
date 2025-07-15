using RonWeb.API.Models.Code;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeHelper
    {
        public Task<GetCodeResponse> GetListAsync(string codeTypeId, int? page);
        public Task<VwCode> GetAsync(long id);
        public Task CreateAsync(CreateCodeRequest data);
        public Task UpdateAsync(long id, UpdateCodeRequest data);
        public Task DeleteAsync(long data);
    }
}

