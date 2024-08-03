namespace RonWeb.API.Enum
{
    /// <summary>
    /// 環境變數管理
    /// </summary>
	public enum EnvVarEnum
    {
        /// <summary>
        /// CorsPolicy 用,分割
        /// </summary>
        ORIGINS,
        /// <summary>
        /// JWTKey
        /// </summary>
        JWTKEY,
        /// <summary>
        /// ISSUER jwt簽發者
        /// </summary>
        ISSUER,
        /// <summary>
        /// AUDIENCE jwt接收人
        /// </summary>
        AUDIENCE,
        /// <summary>
        /// MySql連線字串
        /// </summary>
        RON_WEB_MYSQL_DB_CONSTR,
        /// <summary>
        /// 加密IV
        /// </summary>
        AESIV,
        /// <summary>
        /// 加密Key
        /// </summary>
        AESKEY,
        ///// <summary>
        ///// 系統接收人地址
        ///// </summary>
        //ERROR_LOG_EMAIL_ADDRESS,
        ///// <summary>
        ///// Gamil地址
        ///// </summary>
        //GMAIL_ADDRESS,
        ///// <summary>
        ///// Gamil顯示名稱
        ///// </summary>
        //GMAIL_DISPLAY_NAME,
        ///// <summary>
        ///// Gmail 發送人
        ///// </summary>
        //GMAIL_SENDER_EMAIL,
        ///// <summary>
        ///// Gmail 應用程式密碼
        ///// </summary>
        //GMAIL_PWD,
        ///// <summary>
        ///// reCAPTCHA v3 Token
        ///// </summary>
        //RE_CAPTCHA_SERVER_TOKEN,
        /// <summary>
        /// FireBaseStorage
        /// </summary>
        STORAGE_BUCKET,
        ///// <summary>
        ///// Redis連線字串
        ///// </summary>
        //RON_WEB_REDIS_DB_CONSTR
        /// <summary>
        /// 合法Host用;設定多個
        /// </summary>
        ValidHosts
    }
}

