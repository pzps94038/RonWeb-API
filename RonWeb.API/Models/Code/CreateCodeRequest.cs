namespace RonWeb.API.Models.Code
{
    public class CreateCodeRequest
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
