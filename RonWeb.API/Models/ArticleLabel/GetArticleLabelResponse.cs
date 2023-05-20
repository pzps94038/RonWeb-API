using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.ArticleLabel
{
	public class GetArticleLabelResponse
	{
        /// <summary>
        /// 標籤總數
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 標籤列表
        /// </summary>
        public List<Label> Labels { get; set; } = new List<Label>();
    }
}

