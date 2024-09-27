using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 文章
/// </summary>
public partial class Article
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
    /// 瀏覽數
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 創建人員
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// 修改日期
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    /// 修改人員
    /// </summary>
    public long? UpdateBy { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    public string Flag { get; set; } = null!;
}
