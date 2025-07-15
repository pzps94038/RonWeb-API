using System;
using RonWeb.API.Models.CodeType;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCodeType
{
    public interface IAdminCodeTypeHelper
    {
        public Task<GetCodeTypeResponse> GetListAsync(int? page);
        public Task<CodeType> GetAsync(long id);
        public Task CreateAsync(CreateCodeTypeRequest data);
        public Task UpdateAsync(long id, UpdateCodeTypeRequest data);
        public Task DeleteAsync(long data);
    }
}

