using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

/// <summary>
/// 專案經歷
/// </summary>
public partial class ProjectExperience
{
    /// <summary>
    /// 專案經歷Id
    /// </summary>
    public int ProjectExperienceId { get; set; }

    /// <summary>
    /// 專案名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 專案描述
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 專案貢獻
    /// </summary>
    public string Contributions { get; set; } = null!;

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
