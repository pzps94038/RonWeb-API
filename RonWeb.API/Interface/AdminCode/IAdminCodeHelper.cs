using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Code;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeHelper :
        IGetByIdAsync<long, VwCode>,
        ICreateAsync<CreateCodeRequest>,
        IUpdateAsync<long, UpdateCodeRequest>,
        IDeleteAsync<long>
    {
        public Task<GetCodeResponse> GetListAsync(string codeTypeId, int? page);
    }
}

