using System;
using Microsoft.Extensions.Hosting;
using RonWeb.API.Enum;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Upload;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.API.Models.Upload;
using RonWeb.Core;

namespace RonWeb.API.Helper.Upload
{
	public class UploadHelper: IUploadHelper
    {

        public async Task<UploadFileResponse> UploadFile(IFormFile file)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg" };
            string fileExtension = Path.GetExtension(file.FileName); // 取得檔案附檔名
            if (allowedExtensions.Contains(fileExtension))
            {
                long maxSizeInBytes = 20 * 1024 * 1024; // 20MB 限制
                if (file.Length <= maxSizeInBytes)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                        string fileName = Guid.NewGuid().ToString();
                        string path = @$"{DateTime.Now.ToString("yyyy-MM-dd")}/Artticle/{fileName}{fileExtension}";
                        var res = await new FireBaseStorageTool(storageBucket).Upload(stream, path);
                        return new UploadFileResponse()
                        {
                            Path = res.Path,
                            Url = res.Url,
                            FileName = fileName
                        };
                    }
                }
                else
                {
                    throw new FileSizeException("檔案大小超過上限，請選擇小於 20MB 的檔案");
                }
            }
            else
            {
                throw new ImgExtensionException();
            }
            
        }
    }
}

