using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// Token紀錄表
/// </summary>
public partial class RefreshTokenLog
{
    /// <summary>
    /// 刷新Token
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Token擁有人
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 過期日期
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    public virtual UserMain User { get; set; } = null!;
}
