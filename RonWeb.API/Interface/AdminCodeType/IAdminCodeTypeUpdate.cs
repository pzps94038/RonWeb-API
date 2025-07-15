using System;
using RonWeb.API.Models.CodeType;

namespace RonWeb.API.Interface.AdminCodeType
{
    public interface IAdminCodeTypeUpdate
    {
        public Task UpdateAsync(long id, UpdateCodeTypeRequest data);
    }
}