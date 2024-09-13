using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using System.Text.Json;

namespace RonWeb.API.Middleware
{
    public class ExceptionHandlerMiddleware : IExceptionHandlerMiddleware
    {
        private readonly ILogHelper _logger;
        public ExceptionHandlerMiddleware(ILogHelper logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UniqueException ex)
            {
                await HandleExceptionAsync(context, ex, ReturnCode.Unique, ReturnMessage.Unique);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, ReturnCode.NotFound, ReturnMessage.NotFound);
            }
            catch (AuthExpiredException ex)
            {
                await HandleExceptionAsync(context, ex, ReturnCode.AuthExpired, ReturnMessage.AuthExpired);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, ReturnCode.Fail, ReturnMessage.SystemFail);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ReturnCode returnCode, ReturnMessage returnMessage)
        {
            _logger.Error(ex);
            context.Response.ContentType = "application/json; charset=utf-8";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(new BaseResponse(returnCode.Description(), returnMessage.Description()), options);
            await context.Response.WriteAsync(json);

        }
    }
}
