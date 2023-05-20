using System;
namespace RonWeb.API.Models.ArticleLabel
{
	public class CreateArticleLabelRequest
	{
        /// <summary>
        /// 標籤名稱
        /// </summary>
        public string LabelName { get; set; } = string.Empty;

        /// <summary>
        /// 建立人
        /// </summary>
        public long UserId { get; set; }
    }
}

