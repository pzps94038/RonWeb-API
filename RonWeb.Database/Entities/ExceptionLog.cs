using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 錯誤Log
/// </summary>
public partial class ExceptionLog
{
    /// <summary>
    /// 主鍵Id
    /// </summary>
    public long LogId { get; set; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Stack
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// 日誌等級
    /// </summary>
    public string Level { get; set; } = null!;

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }
}
