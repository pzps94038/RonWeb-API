namespace RonWeb.API.Models.CustomizeException
{
    public class AuthExpiredException: Exception
    {
        public AuthExpiredException() : base("身分驗證已過期") { }
        public AuthExpiredException(string msg) : base(msg) { }
    }
}
