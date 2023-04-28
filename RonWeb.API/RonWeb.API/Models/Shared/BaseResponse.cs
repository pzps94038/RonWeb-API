namespace RonWeb.API.Models.Shared
{
    public class BaseResponse<T>
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string ReturnMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
