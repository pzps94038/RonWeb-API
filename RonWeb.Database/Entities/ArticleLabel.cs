﻿using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 文章標籤
/// </summary>
public partial class ArticleLabel
{
    /// <summary>
    /// 標籤Id
    /// </summary>
    public long LabelId { get; set; }

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

    /// <summary>
    /// 標籤名稱
    /// </summary>
    public string LabelName { get; set; } = null!;
}
