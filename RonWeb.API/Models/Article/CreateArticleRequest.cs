using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
    public class CreateArticleRequest
    {
        /// <summary>
        ///  文章標題
        /// </summary>
        public string ArticleTitle { get; set; } = string.Empty;

        /// <summary>
        ///  預覽內容
        /// </summary>
        public string PreviewContent { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string Flag { get; set; } = "Y";

        /// <summary>
        /// 分類ID
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 創建人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 預覽上傳圖
        /// </summary>
        public List<UploadFile> PrevFiles { get; set; } = new List<UploadFile>();

        /// <summary>
        /// 內容上傳圖
        /// </summary>
        public List<UploadFile> ContentFiles { get; set; } = new List<UploadFile>();

        /// <summary>
        /// 標籤列表
        /// </summary>
        public List<Label> Labels { get; set; } = new List<Label>();

        /// <summary>
        /// 參考文章
        /// </summary>
        public List<string> References { get; set; } = new List<string>();
    }
}
