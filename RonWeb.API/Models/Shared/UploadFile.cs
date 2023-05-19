using System;
namespace RonWeb.API.Models.Shared
{
    public class UploadFile
    {
        /// <summary>
        /// 檔名
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// 倉庫位置
        /// </summary>
        public string Path { get; set; } = string.Empty;
        /// <summary>
        /// 網路位置
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}

