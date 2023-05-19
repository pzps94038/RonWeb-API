namespace RonWeb.API.Models.CustomizeException
{
    public class ImgExtensionException : Exception
    {
        public ImgExtensionException() : base("只允許上傳以下類型的檔案: jpg, jpeg, png, gif, bmp, tiff, svg") { }
        public ImgExtensionException(string msg) : base(msg) { }
    }
}
