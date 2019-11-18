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

namespace CIM.API.IntegrationTests
{

    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

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
        }
    }
}
