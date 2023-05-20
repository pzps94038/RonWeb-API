namespace RonWeb.API.Models.RefreshToken
{
    public class RefreshTokenRequest
    {
        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }
    }
}
