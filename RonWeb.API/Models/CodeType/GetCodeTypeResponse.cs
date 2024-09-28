namespace RonWeb.API.Models.CodeType
{
    public class GetCodeTypeResponse
    {
        /// <summary>
        /// 代碼類型總數
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 代碼列表
        /// </summary>
        public List<RonWeb.Database.Entities.CodeType> CodeTypes { get; set; } = new List<RonWeb.Database.Entities.CodeType>();
    }
}

