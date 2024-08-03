using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Enum;
using RonWeb.Core;

namespace RonWeb.API.Filter
{
    public class HostFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validHostEnv = Environment.GetEnvironmentVariable(EnvVarEnum.ValidHosts.Description()) ?? "";
            var validHosts = validHostEnv.Split(';');
            var requestHost = context.HttpContext.Request.Host.Host;

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
