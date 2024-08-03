using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Enum;
using RonWeb.Core;

namespace RonWeb.API.Filter
{
    public class HostFilter : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HostFilter(
            IHttpContextAccessor httpContextAccessor
        )
        { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var validHostEnv = Environment.GetEnvironmentVariable(EnvVarEnum.ValidHosts.Description()) ?? "";
            var validHosts = validHostEnv.Split(';');
            var requestHost = context.HttpContext.Request.Host.Host;
            Console.WriteLine("Host:" + context.HttpContext.Request.Host.Value);
            Console.WriteLine("Header IP:" + context.HttpContext.Request.Headers.Host);

            Console.WriteLine("httpContextAccessor host", _httpContextAccessor.HttpContext.Request.Host);

            if (!validHosts.Contains(requestHost))
            {
                context.Result = new ContentResult
                {
                    Content = "Invalid Host",
                    StatusCode = 403
                };
                Console.WriteLine($"Invalid Host: {requestHost}");
            }

            base.OnActionExecuting(context);
        }
    }
}
