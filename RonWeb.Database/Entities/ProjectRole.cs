using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 專案角色
/// </summary>
public partial class ProjectRole
{
    /// <summary>
    /// 專案經歷Id
    /// </summary>
    public int ProjectExperienceId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public string RoleId { get; set; } = null!;

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
    /// 主鍵
    /// </summary>
    public long Id { get; set; }
}
