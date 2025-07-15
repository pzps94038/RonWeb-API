using RonWeb.API.Models.Code;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeHelper :
        IAdminCodeGet,
        IAdminCodeCreate,
        IAdminCodeUpdate,
        IAdminCodeDelete
    {
        public Task<GetCodeResponse> GetListAsync(string codeTypeId, int? page);
    }
}

