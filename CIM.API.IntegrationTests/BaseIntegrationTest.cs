using CIM.API.IntegrationTests.DbModel;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CIM.API.IntegrationTests
{

    public class BaseIntegrationTest
    {

        public TestScenario CreateWebApplication()
        {
            var scenario = new TestScenario();
            scenario.AppFactory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<cim_3m_1Context>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<cim_3m_1Context>((options, context) =>
                    {
                        context.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    var masterDataService = new Mock<IMasterDataService>();
                    var moqMasterData = this.GetMoqMasterData(scenario.TestDb);
                    masterDataService.Setup(x => x.GetData()).Returns(Task.FromResult(moqMasterData));
                    masterDataService.Setup(x => x.Refresh(Constans.MasterDataType.All)).Returns(Task.FromResult(moqMasterData));

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                    services.SwapTransient(services => masterDataService.Object);

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<cim_3m_1Context>();

                        // Ensure the database is created.
                        db.Database.EnsureCreated();

                        var adminGroup = new UserGroups
                        {
                            IsActive = true,
                            Name = "Admin"
                        };
                        var userService = scopedServices.GetRequiredService<IUserService>();
                        //userService.CurrentUser = new CurrentUserModel { LanguageId = "en", IsValid = true, UserId = "MockTestId" };
                        //db.UserGroups.Add(adminGroup);
                        db.SaveChanges();
                        var registerUserModel = new UserModel
                        {
                            Email = "test@email.com",
                            UserName = "admin" + RandomString(),
                            Password = "super-secret",
                            FirstName = "Hans",
                            LastName = "Meier",
                            DefaultLanguageId = "en",
                            //Image = null,
                            UserGroupId = adminGroup.Id,
                        };
                        //userService.Create(registerUserModel);
                        scenario.Admin = registerUserModel;
                        scenario.Admin.Id = db.Users.Where(x => x.UserName == scenario.Admin.UserName).Select(x => x.Id).FirstOrDefault();
                        scenario.AdminToken = userService.CreateToken(scenario.Admin.Id).Result;
                    }

                });
            });
            scenario.TestClient = scenario.AppFactory.CreateClient();
            scenario.ServiceScopeFactory = scenario.AppFactory.Services.GetService<IServiceScopeFactory>();
            return scenario;
        }

        public void CleanDb(TestScenario seacario)
        {

            //Start_Test
            using (var scope = seacario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_3m_1Context>();
                //

                foreach (var item in context.ProductionPlan.ToList())
                {
                    context.Entry(item).State = EntityState.Deleted;
                }

                foreach (var item in context.Product.ToList())
                {
                    context.Entry(item).State = EntityState.Deleted;
                }

                //foreach (var item in context.Route.ToList())
                //{
                //    context.Entry(item).State = EntityState.Deleted;
                //}

                context.SaveChanges();

            }

        }


        public MasterDataModel GetMoqMasterData(TestDbModel testDb)
        {

            var machines = testDb.MachineDb.ToDictionary(x => x.Id, x => MapperHelper.AsModel(x, new MachineModel()));

            var routes = testDb.RoutesDb.ToDictionary(x => x.Id, x => MapperHelper.AsModel(x, new RouteModel
            {
                MachineList = machines
            }));

            return new MasterDataModel
            {
                Routes = routes
            }; ;
        }

        protected HttpContent GetHttpContentForPost<T>(T model, string token)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("token", token);
            return content;
        }


        public string RandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

    }

    public static class ServiceCollectionExtionsion
    {

        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Transient"/> registrations of <see cref="TService"/> and adds a new registration which uses the <see cref="Func{IServiceProvider, TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <param name="services"></param>
        /// <param name="implementationFactory">The implementation factory for the specified type.</param>
        public static void SwapTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
        {
            if (services.Any(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Transient))
            {
                var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Transient).ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    services.Remove(serviceDescriptor);
                }
            }

            services.AddTransient(typeof(TService), (sp) => implementationFactory(sp));
        }

    }

}
