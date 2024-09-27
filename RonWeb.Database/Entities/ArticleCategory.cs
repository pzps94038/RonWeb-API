using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 文章類別
/// </summary>
public partial class ArticleCategory
{
    /// <summary>
    /// 類別Id
    /// </summary>
    public long CategoryId { get; set; }

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
    /// 類別名稱
    /// </summary>
    public string CategoryName { get; set; } = null!;
}
