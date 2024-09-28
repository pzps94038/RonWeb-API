using System;
using RonWeb.API.Models.Shared;
using RonWeb.API.Models.Upload;

namespace RonWeb.API.Interface.Upload
{
    public interface IUploadHelper
    {
        public Task<UploadFileResponse> UploadArticleFile(IFormFile file);
        public Task<UploadFileResponse> UploadProjectExperienceFile(IFormFile file);
    }
}

