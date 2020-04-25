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
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<cim_dbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CIMDatabase")));

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IUnitOfWorkCIM, UnitOfWorkCIM>();

            services.AddTransient<ISiteRepository, SiteRepository>();
            services.AddTransient<IUserAppTokenRepository, UserAppTokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDirectSqlRepository, DirectSqlRepository>();
            services.AddTransient<IProductionPlanRepository, ProductionPlanRepository>();
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ILossLevel3Repository, LossLevel3Repository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddTransient<IMachineComponentRepository, MachineComponentRepository>();
            services.AddTransient<IRouteMachineRepository, RouteMachineRepository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddTransient<IMachineComponentLossRepository, RecordMachineComponentLossRepository>();
            services.AddTransient<IMachineComponentStatusRepository, RecordMachineComponentStatusRepository>();
            services.AddTransient<IProductionOutputRepository, RecordProductionOutputRepository>();
            services.AddTransient<IProductionStatusRepository, ProductionStatusRepository>();
            services.AddTransient<IUnitsRepository, UnitsRepository>();
            services.AddTransient<IRouteProductGroupRepository, RouteProductGroupRepository>();
            services.AddTransient<IRecordManufacturingLossRepository, RecordManufacturingLossRepository>();
            services.AddTransient<IRecordProductionPlanWasteRepository, RecordProductionPlanWasteRepository>();

            services.AddTransient<IProductionPlanService, ProductionPlanService>();
            services.AddTransient<IDirectSqlService, DirectSqlService>();
            services.AddTransient<ICipherService, CipherService>();
            services.AddTransient<ISiteService, SiteService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMachineComponentService, MachineComponentService>();
            services.AddTransient<IMachineAlertService, MachineAlertService>();
            services.AddTransient<IActiveProductionPlanService, ActiveProductionPlanService>();
            services.AddTransient<IRecordManufacturingLossService, RecordManufacturingLossService>();
            services.AddTransient< IRecordProductionPlanWasteService, RecordProductionPlanWasteService>();

            services.AddTransient<IMasterDataService, MasterDataService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<ILossLevel3Service, LossLevel3Service>();

            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CIM Data Service API", Version = "v1" });
            });

            services.InstallServicesInAssembly(Configuration);

            var sp = services.BuildServiceProvider();
            var masterDataService = sp.GetService<IMasterDataService>();
            masterDataService.Refresh();

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

            app.UseCors(builder =>
               builder.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChartHub>("/chart");
                endpoints.MapHub<MachineHub>("/activeprocess");
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
#if (DEBUG)
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CIM Data Service");
#else
                                c.SwaggerEndpoint("/cim-dev-api/swagger/v1/swagger.json", "CIM Data Service");
#endif
            });

        }
    }
}
