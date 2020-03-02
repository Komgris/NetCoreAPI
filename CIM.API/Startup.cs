using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Installer;
using CIM.API.HubConfig;
using CIM.BusinessLogic;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.DAL.Implements;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CIM.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
                      Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<cim_dbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CIMDatabase")));

            services.AddTransient<IUnitOfWorkCIM, UnitOfWorkCIM>();

            services.AddTransient<ISiteRepository, SiteRepository>();
            services.AddTransient<IUserAppTokenRepository, UserAppTokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDirectSqlRepository, DirectSqlRepository>();
            services.AddTransient<IProductionPlanRepository, ProductionPlanRepository>();

            services.AddTransient<IProductionPlanService, ProductionPlanService>();
            services.AddTransient<IDirectSqlService, DirectSqlService>();
            services.AddTransient<ICipherService, CipherService>();
            services.AddTransient<ISiteService, SiteService>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CIM Data Service API", Version = "v1" });
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                builder =>
                                {
                                    builder.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                                });
            });

            services.InstallServicesInAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChartHub>("/chart");
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CIM Data Service");
            });
           

        }
    }
}
