using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CIM.API.Controllers
{
    public class MyCustomAuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public MyCustomAuthenticationMiddleware(
            //IUserService userService,
            RequestDelegate next)
        {
            //_userService = userService;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["token"];
            var userService = (IUserService)context.RequestServices.GetService(typeof(IUserService));
            if (!string.IsNullOrEmpty(token.ToString()))
            {
                var currentUserModel = userService.GetCurrentUserModel(token);
                context.Items.Add(Constans.CURRENT_USER, currentUserModel);
                if (currentUserModel.IsValid)
                {
                    await this.next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }

    public static class MyCustomAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyCustomAuthenticationMiddleware>();
        }
    }

    public class MyCustomAuthenticationMiddlewarePipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMyCustomAuthentication();
        }
    }
}