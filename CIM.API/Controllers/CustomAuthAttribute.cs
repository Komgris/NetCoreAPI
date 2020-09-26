using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CIM.API.Controllers
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate next;
        private IConfiguration _configuration;
        public CustomAuthenticationMiddleware(
            RequestDelegate next,
            IConfiguration configuration
            )
        {
            this.next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["token"];
            var appId = Convert.ToInt32(context.Request.Headers["appId"]);

            var enabledVerifyToken = _configuration.GetValue<bool>("EnabledVerifyToken");
            if (enabledVerifyToken)
            {
                var userService = (IUserService)context.RequestServices.GetService(typeof(IUserService));
                if (userService.GetCurrentUserModel(token, appId).Result)
                {
                    await this.next.Invoke(context);
                }
                else
                {
                    var model = new ProcessReponseModel<object>
                    {
                        IsAuthorized = false,
                        Message = "Unauthorized"
                    };
                    var result = new ObjectResult(model);
                    await context.WriteResultAsync(result);
                }
            }
            else
            {
                BaseService.CurrentUserId = _configuration.GetValue<string>("DefaultUserId");
                await this.next.Invoke(context);
            }
        }
    }

    public static class HttpContextExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        public static Task WriteResultAsync<TResult>(this HttpContext context, TResult result)
            where TResult : IActionResult
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.RequestServices.GetService<IActionResultExecutor<TResult>>();

            if (executor == null)
            {
                throw new InvalidOperationException($"No result executor for '{typeof(TResult).FullName}' has been registered.");
            }

            var routeData = context.GetRouteData() ?? EmptyRouteData;

            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }
    }

    public static class CustomAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthenticationMiddleware>();
        }
    }

    public class CustomAuthenticationMiddlewarePipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMyCustomAuthentication();
        }
    }
}