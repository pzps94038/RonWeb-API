using System;
using RonWeb.API.Models.CodeType;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCodeType
{
    public interface IAdminCodeTypeHelper :
        IAdminCodeTypeGet,
        IAdminCodeTypeCreate,
        IAdminCodeTypeUpdate,
        IAdminCodeTypeDelete
    {
        public Task<GetCodeTypeResponse> GetListAsync(int? page);
    }
}

