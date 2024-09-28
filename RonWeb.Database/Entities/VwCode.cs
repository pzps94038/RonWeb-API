﻿using System;
using System.Collections.Generic;

namespace RonWeb.Database.Entities;

public partial class VwCode
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
    /// 主鍵
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 代碼Id
    /// </summary>
    public string CodeId { get; set; } = null!;

    /// <summary>
    /// 代碼名稱
    /// </summary>
    public string CodeName { get; set; } = null!;

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