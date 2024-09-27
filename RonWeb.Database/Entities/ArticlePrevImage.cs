using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 預覽文章圖片
/// </summary>
public partial class ArticlePrevImage
{
    /// <summary>
    /// 預覽文章圖片Id
    /// </summary>
    public long ImageId { get; set; }

    /// <summary>
    /// 文章Id
    /// </summary>
    public long ArticleId { get; set; }

    /// <summary>
    /// 黨案名稱
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// 路徑
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// 對外網址
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 創建人員
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    /// 更新人員
    /// </summary>
    public long? UpdateBy { get; set; }
}
