﻿using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 文章圖片
/// </summary>
public partial class ArticleImage
{
    /// <summary>
    /// 圖片Id
    /// </summary>
    public long ImageId { get; set; }

    /// <summary>
    /// 文章Id
    /// </summary>
    public long ArticleId { get; set; }

    /// <summary>
    /// 檔案名稱
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
    /// 修改日期
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    /// 修改人員
    /// </summary>
    public long? UpdateBy { get; set; }
}
