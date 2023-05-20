using System;
namespace RonWeb.API.Models.ArticleLabel
{
	public class UpdateArticleLabelRequest
	{
        /// <summary>
        /// 標籤ID
        /// </summary>
        public long LabelId { get; set; }

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

