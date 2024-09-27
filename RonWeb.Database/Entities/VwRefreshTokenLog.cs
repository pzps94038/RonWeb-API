using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

public partial class VwRefreshTokenLog
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
    /// 帳號
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;
}
