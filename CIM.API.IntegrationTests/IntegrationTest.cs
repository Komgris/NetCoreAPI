using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CIM.Domain.Models;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;

namespace CIM.API.IntegrationTests
{

    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected cim_dbContext DbContext;
        protected IServiceScopeFactory ServiceScopeFactory;
        public UserModel Admin { get; set; }
        public string AdminToken { get; set; }

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder( builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the app's ApplicationDbContext registration.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<cim_dbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add ApplicationDbContext using an in-memory database for testing.
                    services.AddDbContext<cim_dbContext>((options, context) =>
                    {
                        context.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database
                    // context (ApplicationDbContext).
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<cim_dbContext>();
                        //var logger = scopedServices
                        //    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        // Ensure the database is created.
                        db.Database.EnsureCreated();

                        var adminGroup = new UserGroups
                        {
                            IsActive = true,
                            Name = "Admin"
                        };
                        var userService = scopedServices.GetRequiredService<IUserService>();
                        userService.CurrentUser = new CurrentUserModel { LanguageId = "en", IsValid = true, UserId = "MockTestId" };
                        db.UserGroups.Add(adminGroup);
                        db.SaveChanges();
                        var registerUserModel = new UserModel
                        {
                            Email = "test@email.com",
                            UserName = "admin",
                            Password = "super-secret",
                            FirstName = "Hans",
                            LastName = "Meier",
                            LanguageId = "en",
                            Image = null,
                            UserGroupId = adminGroup.Id,
                        };
                        userService.Register(registerUserModel);
                        Admin = registerUserModel;
                        Admin.Id = db.Users.Where(x => x.UserName == Admin.UserName).Select(x => x.Id).FirstOrDefault();
                        AdminToken = userService.CreateToken(Admin.Id).Result;
                        //try
                        //{
                        //    // Seed the database with test data.
                        //    Utilities.InitializeDbForTests(db);
                        //}
                        //catch (Exception ex)
                        //{
                        //    //logger.LogError(ex, "An error occurred seeding the " +
                        //    //    "database with test messages. Error: {Message}", ex.Message);
                        //}
                    }

                });
            });
            TestClient = appFactory.CreateClient();
            ServiceScopeFactory = appFactory.Services.GetService<IServiceScopeFactory>();
        }

        protected HttpContent GetHttpContentForPost<T>(T model, string token)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("token", token);
            return content;
        }
    }
}
