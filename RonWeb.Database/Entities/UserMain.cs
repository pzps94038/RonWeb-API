using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 使用者
/// </summary>
public partial class UserMain
{
    /// <summary>
    /// 使用者Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 修改日期
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<RefreshTokenLog> RefreshTokenLog { get; set; } = new List<RefreshTokenLog>();
}
