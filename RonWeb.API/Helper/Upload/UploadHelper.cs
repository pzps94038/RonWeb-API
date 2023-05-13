using System;
using Microsoft.Extensions.Hosting;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Upload;
using RonWeb.Core;

namespace RonWeb.API.Helper.Upload
{
	public class UploadHelper: IUploadHelper
    {

        public async Task<string> UploadFile(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                var url = await new FireBaseStorageTool(storageBucket).Upload(stream, new List<string>() {
                    DateTime.Now.ToString("yyyy/MM/dd"),
                    "Artticle",
                    Guid.NewGuid().ToString()
                });
                return url!;
            }
        }
    }
}

