using System;
using RonWeb.API.Models.Code;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeCreate
    {
        public Task CreateAsync(CreateCodeRequest data);
    }
}