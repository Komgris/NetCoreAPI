using CIM.API.Installer;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.DAL.Implements;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using System.IO;
using CIM.Model;

namespace CIM.API {
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BaseService.CurrentUserId = Configuration.GetValue<string>("DefaultUserId");
            BaseService.ExcelMapping = new ExcelMappingModel();
            Configuration.GetSection(nameof(ExcelMappingModel)).Bind(BaseService.ExcelMapping);//.Bind(BaseService.ExcelMapping);
#if (DEBUG)
            BaseService.ImagePath = Directory.GetCurrentDirectory()+"\\Image";
            BaseService.DocPath = Directory.GetCurrentDirectory() + "\\doc";
#else
            BaseService.ImagePath =  Configuration.GetValue<string>("ServerPath");
            BaseService.DocPath =  Configuration.GetValue<string>("DocPath");
#endif

        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddDbContext<cim_3m_1Context>(options =>
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
            services.AddTransient<ILossLevel1Repository, LossLevel1Repository>();
            services.AddTransient<ILossLevel2Repository, LossLevel2Repository>();
            services.AddTransient<ILossLevel3Repository, LossLevel3Repository>();
            services.AddTransient<IMachineTypeLossLevel3Repository, MachineTypeLossLevel3Repository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddTransient<IRouteMachineRepository, RouteMachineRepository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddTransient<IRecordProductionPlanOutputRepository, RecordProductionPlanOutputRepository>();
            services.AddTransient<IProductionStatusRepository, ProductionStatusRepository>();
            services.AddTransient<IUnitsRepository, UnitsRepository>();
            services.AddTransient<IRouteProductGroupRepository, RouteProductGroupRepository>();
            services.AddTransient<IRecordManufacturingLossRepository, RecordManufacturingLossRepository>();
            services.AddTransient<IRecordProductionPlanWasteRepository, RecordProductionPlanWasteRepository>();
            services.AddTransient<IRecordMachineStatusRepository, RecordMachineStatusRepository>();
            services.AddTransient<IMachineOperatorRepository, MachineOperatorRepository>();

            services.AddTransient<IWasteLevel1Repository, WasteLevel1Repository>();
            services.AddTransient<IWasteLevel2Repository, WasteLevel2Repository>();
            services.AddTransient<IMachineTypeRepository, MachineTypeRepository>();
            services.AddTransient<IRecordProductionPlanWasteMaterialRepository, RecordProductionPlanWasteMaterialRepository>();
            services.AddTransient<IMachineTypeComponentTypeRepository, MachineTypeComponentTypeRepository>();
            services.AddTransient<IRecordActiveProductionPlanRepository, RecordActiveProductionPlanRepository>();
            services.AddTransient<IProductMaterialRepository, ProductMaterialRepository>();
            services.AddTransient<IBomMaterialRepository, BomMaterialRepository>();
            services.AddTransient<IBomRepository, BomRepository>();
            services.AddTransient<IProductFamilyRepository, ProductFamilyRepository>();
            services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
            services.AddTransient<IMaterialTypeRepository, MaterialTypeRespository>();
            services.AddTransient<IEmployeesRepository, EmployeesRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITeamTypeRepository, TeamTypeRepository>();
            services.AddTransient<ITeamEmployeesRepository, TeamEmployeesRepository>();
            services.AddTransient<IMachineTeamRepository, MachineTeamRepository>();
            services.AddTransient<IRecordMaintenancePlanRepository, RecordMaintenancePlanRepository>();
            services.AddTransient<IUserPositionRepository, UserPositionRepository>();
            services.AddTransient<INameRepository, NameRepository>();
            services.AddTransient<IEducationRepository, EducationRepository>();
            services.AddTransient<IUserGroupRepository, UserGroupRepository>();
            services.AddTransient<IProcessTypeRepository, ProcessTypeRepository>();
            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<IAppFeatureRepository, AppFeatureRepository>();
            services.AddTransient<IUserGroupAppRepository, UserGroupAppRepository>();
            services.AddTransient<IUserGroupAppFeatureRepository, UserGroupAppFeatureRepository>();
            services.AddTransient<IWasteNonePrimeRepository, WasteNonePrimeRepository>();
            services.AddTransient<ISystemParameterRepository, SystemParameterRepository>();
            services.AddTransient<IProductionPlanCheckListRepository, ProductionPlanCheckListRepository>();
            services.AddTransient<IRecordProductionPlanCheckListRepository, RecordProductionPlanCheckListRepository>();
            services.AddTransient<IRecordProductionPlanCheckListDetailRepository, RecordProductionPlanCheckListDetailRepository>();
            services.AddTransient<IWasteRepository, WasteRepository>();

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
            services.AddTransient<IRecordProductionPlanWasteService, RecordProductionPlanWasteService>();
            services.AddTransient<IMachineTypeService, MachineTypeService>();
            services.AddTransient<IRouteService, RouteService>();
            services.AddTransient<IRecordProductionPlanOutputService, RecordProductionPlanOutputService>();

            services.AddTransient<IMasterDataService, MasterDataService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<ILossLevel1Service, LossLevel1Service>();
            services.AddTransient<ILossLevel2Service, LossLevel2Service>();
            services.AddTransient<ILossLevel3Service, LossLevel3Service>();
            services.AddTransient<IMachineTypeLossLevel3Service, MachineTypeLossLevel3Service>();
            services.AddTransient<IBomService, BomService>();
            services.AddTransient<IUtilitiesService, UtilitiesService>();
            services.AddTransient<IProductGroupService, ProductGroupService>();
            services.AddTransient<IEmployeesService, EmployeesService>();
            services.AddTransient<IMachineOperatorService, MachineOperatorService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IRecordMaintenancePlanService, RecordMaintenancePlanService>();
            services.AddTransient<IUserGroupService, UserGroupService>();
            services.AddTransient<IUserGroupAppService, UserGroupAppService>();
            services.AddTransient<IUserGroupAppFeatureService, UserGroupAppFeatureService>();
            services.AddTransient<IHardwareInterfaceService, HardwareInterfaceService>();
            services.AddTransient<IWasteLevel1Service, WasteLevel1Service>();
            services.AddTransient<IWasteLevel2Service, WasteLevel2Service>();
            services.AddTransient<ISystemParameterService, SystemParameterService>();
            services.AddTransient<ITriggerQueueService, TriggerQueueService>();
            services.AddTransient<IProductionPlanCheckListService, ProductionPlanCheckListServices>();
            services.AddTransient<IRecordProductionPlanCheckListService, RecordProductionPlanCheckListService>();

            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CIM Data Service API", Version = "v1" });
            });

            services.InstallServicesInAssembly(Configuration);

            var sp = services.BuildServiceProvider();
            var masterDataService = sp.GetService<IMasterDataService>();
            masterDataService.Refresh(Model.Constans.MasterDataType.All);
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
               builder.WithOrigins(new[] { "http://localhost:4200", "http://localhost:4401", "http://localhost:4402" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GlobalHub>("/GlobalBoardcast");
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
                c.SwaggerEndpoint("/3m-api/swagger/v1/swagger.json", "CIM Data Service");
#endif
            });

#if (DEBUG)
            //using static path
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), "Image")),
                RequestPath = "/Image"
            });
#endif
        }
    }
}
