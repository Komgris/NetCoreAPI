using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            IConfiguration configuration)
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
                if (!string.IsNullOrEmpty(token.ToString()))
                {
                    var currentUserModel = await userService.GetCurrentUserModel(token, appId);
                    //context.Items.Add(Constans.CURRENT_USER, currentUserModel);
                    //if (currentUserModel.IsValid)
                    //{
                    //    await this.next.Invoke(context);
                    //}
                    //else
                    //{
                    //    context.Response.StatusCode = 401;
                    //}
                }
                //else
                //{
                    //context.Response.StatusCode = 401;
                //}
            }
            else
            {
                BaseService.IsVerifyTokenPass = true;
            }

            await this.next.Invoke(context);
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