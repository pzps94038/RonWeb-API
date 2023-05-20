namespace RonWeb.API.Models.Shared
{
    public class Category
    {
        /// <summary>
        /// 分類ID
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 分類名稱
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
