using System;
using RonWeb.API.Models.CodeType;

namespace RonWeb.API.Interface.AdminCodeType
{
    public interface IAdminCodeTypeCreate
    {
        public Task CreateAsync(CreateCodeTypeRequest data);
    }
}