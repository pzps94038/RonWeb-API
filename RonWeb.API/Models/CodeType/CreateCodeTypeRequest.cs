namespace RonWeb.API.Models.CodeType
{
    public class CreateCodeTypeRequest
    {
        /// <summary>
        /// 代碼類型Id
        /// </summary>
        public string CodeTypeId { get; set; } = string.Empty;

        /// <summary>
        /// 代碼類型名稱
        /// </summary>
        public string CodeTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 建立人
        /// </summary>
        public long UserId { get; set; }
    }
}
