using Newtonsoft.Json;
using RonWeb.API.Interface.Shared;

namespace RonWeb.API.Middleware
{
    public class LogHttpMessagesMiddleware : ILogHttpMessagesMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"Request Time: {requestTime}");

            string requestHeaders = JsonConvert.SerializeObject(context.Request.Headers);
            Console.WriteLine($"Request Headers: {requestHeaders}");

            context.Request.EnableBuffering();

            string requestContentType = context.Request.ContentType ?? "";
            Console.WriteLine($"Content Type: {requestContentType}");

            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                string requestContent = await reader.ReadToEndAsync();
                Console.WriteLine($"Request Body: {requestContent}");
                context.Request.Body.Position = 0;
            }

            await next(context);
        }
    }
}
