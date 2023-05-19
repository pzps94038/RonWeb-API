using System;
using Microsoft.Extensions.Hosting;
using RonWeb.API.Enum;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Upload;
using RonWeb.API.Models.Shared;
using RonWeb.API.Models.Upload;
using RonWeb.Core;

namespace RonWeb.API.Helper.Upload
{
	public class UploadHelper: IUploadHelper
    {

        public async Task<UploadFileResponse> UploadFile(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                string fileName = Guid.NewGuid().ToString();
                string path = @$"{DateTime.Now.ToString("yyyy-MM-dd")}/Artticle/{fileName}";
                var res = await new FireBaseStorageTool(storageBucket).Upload(stream, path);
                return new UploadFileResponse()
                {
                    Path = res.Path,
                    Url = res.Url,
                    FileName = fileName
                };
            }
        }
    }
}

