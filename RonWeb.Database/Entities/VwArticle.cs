using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

public partial class VwArticle
{
    /// <summary>
    /// 文章Id
    /// </summary>
    public long ArticleId { get; set; }

    /// <summary>
    /// 文章標題
    /// </summary>
    public string ArticleTitle { get; set; } = null!;

    /// <summary>
    /// 預覽文章內容
    /// </summary>
    public string PreviewContent { get; set; } = null!;

    /// <summary>
    /// 文章內容
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// 類別Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 類別名稱
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// 標籤Id
    /// </summary>
    public long? LabelId { get; set; }

    /// <summary>
    /// 標籤名稱
    /// </summary>
    public string? LabelName { get; set; }

    /// <summary>
    /// 參考連結
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime? LabelCreateDate { get; set; }

    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime? LabelUpdateDate { get; set; }

    /// <summary>
    /// 瀏覽數
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime ArticleCreateDate { get; set; }

    /// <summary>
    /// 修改日期
    /// </summary>
    public DateTime? ArticleUpdateDate { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    public string Flag { get; set; } = null!;
}
