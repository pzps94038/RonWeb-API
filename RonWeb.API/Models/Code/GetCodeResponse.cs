using System;
using RonWeb.API.Models.Shared;
using RonWeb.Database.Entities;
namespace RonWeb.API.Models.ArticleLabel
{
    public class GetCodeResponse
    {
        /// <summary>
        /// 類型Id
        /// </summary>
        public string CodeTypeId { get; set; } = null!;

        /// <summary>
        /// 類型名稱
        /// </summary>
        public string CodeTypeName { get; set; } = null!;

        /// <summary>
        /// 代碼類型總數
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 代碼列表
        /// </summary>
        public List<Code> Codes { get; set; } = new List<Code>();
    }
}

