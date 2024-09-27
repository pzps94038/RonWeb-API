using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 代碼類型表
/// </summary>
public partial class CodeType
{
    /// <summary>
    /// 類型Id
    /// </summary>
    public string CodeTypeId { get; set; } = null!;

    /// <summary>
    /// 類型名稱
    /// </summary>
    public string CodeTypeName { get; set; } = null!;

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
    /// 主鍵
    /// </summary>
    public long Id { get; set; }
}
