using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.Upload
{
	public interface IUploadHelper
	{
		public Task<UploadFileResponse> UploadFile(IFormFile file);
	}
}

