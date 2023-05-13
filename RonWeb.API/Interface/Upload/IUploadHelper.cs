using System;
namespace RonWeb.API.Interface.Upload
{
	public interface IUploadHelper
	{
		public Task<string> UploadFile(IFormFile file);
	}
}

