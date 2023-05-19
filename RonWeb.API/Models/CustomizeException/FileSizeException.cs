namespace RonWeb.API.Models.CustomizeException
{
    public class FileSizeException : Exception
    {
        public FileSizeException(string msg) : base(msg) { }
    }
}
