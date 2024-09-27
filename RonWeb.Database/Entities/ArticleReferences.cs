using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 參考文章表
/// </summary>
public partial class ArticleReferences
{
    /// <summary>
    /// 參考文章Id
    /// </summary>
    public long ArticleReferencesId { get; set; }

    /// <summary>
    /// 文章Id
    /// </summary>
    public long ArticleId { get; set; }

    /// <summary>
    /// 參考連結
    /// </summary>
    public string Link { get; set; } = null!;

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
