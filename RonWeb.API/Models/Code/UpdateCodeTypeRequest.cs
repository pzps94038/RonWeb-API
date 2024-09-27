using RonWeb.API.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace RonWeb.API.Models.Article
{
    public class UpdateCodeRequest
    {
        /// <summary>
        /// 代碼類型Id
        /// </summary>
        public string CodeTypeId { get; set; } = string.Empty;

        /// <summary>
        /// 代碼Id
        /// </summary>
        public string CodeId { get; set; } = string.Empty;

        /// <summary>
        /// 代碼名稱
        /// </summary>
        public string CodeName { get; set; } = string.Empty;

        /// <summary>
        /// 建立人
        /// </summary>
        public long UserId { get; set; }
    }
}
