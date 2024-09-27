using System;
using RonWeb.Database.Entities;
namespace RonWeb.API.Models.ArticleLabel
{
    public class GetCodeResponse
    {
        /// <summary>
        /// 代碼類型總數
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 代碼列表
        /// </summary>
        public List<RonWeb.Database.Entities.Code> Codes { get; set; } = new List<RonWeb.Database.Entities.Code>();
    }
}

