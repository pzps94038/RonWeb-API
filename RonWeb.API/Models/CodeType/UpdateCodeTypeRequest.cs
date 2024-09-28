using RonWeb.API.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace RonWeb.API.Models.CodeType
{
    public class UpdateCodeTypeRequest
    {
        /// <summary>
        /// 類型代碼
        /// </summary>
        public string CodeTypeId { get; set; } = string.Empty;

        /// <summary>
        /// 類型名稱
        /// </summary>
        public string CodeTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 更新人
        /// </summary>
        public long UserId { get; set; }
    }
}
