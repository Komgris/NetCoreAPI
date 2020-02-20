using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using CIM.DAL.Interfaces;
using Moq;
using CIM.BusinessLogic.Services;
using System.Linq;
using CIM.Domain.Models;

namespace CIM.API.IntegrationTests
{
    public class ProductControllerTest : IntegrationTest
    {
        [Fact]
        public async Task InsertProduct_Test()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var productRepository = new Mock<IProductRepository>().Object;
            var service = new ProductService(productRepository, unitOfWork);

            var dbProductMoq = new List<Product>()
            {
                new Product{ Code="testA",ProductFamilyId=1,ProductGroupId=1,ProductTypeId=3 },
                new Product{ Code="testB",ProductFamilyId=2,ProductGroupId=1,ProductTypeId=4 },
                new Product{ Code="testC",ProductFamilyId=2,ProductGroupId=1,ProductTypeId=3 },
            };
            var content = JsonConvert.SerializeObject(dbProductMoq);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.Database.EnsureCreated();

                // Count products in database
                var beforeTest = context.Product;
                var beforeCount = beforeTest.Count();

                context.Product.Add(dbProductMoq[0]);
                context.Product.Add(dbProductMoq[1]);
                context.Product.Add(dbProductMoq[2]);
                context.SaveChanges();

                // Count products after upload
                var afterTest = context.Product;
                var afterCount = afterTest.Count();

                int diffCount = afterCount - beforeCount;

                int moqCount = dbProductMoq.Count();

                // find product test A, B and C
                var findProductA = afterTest.Where(x => x.Code == "testA").ToList().FirstOrDefault();
                var findProductB = afterTest.Where(x => x.Code == "testB").ToList().FirstOrDefault();
                var findProductC = afterTest.Where(x => x.Code == "testC").ToList().FirstOrDefault();

                // Compare product property A
                moqCount.Should().Be(diffCount);

                findProductA.Should().NotBeNull();
                findProductB.Should().NotBeNull();
                findProductC.Should().NotBeNull();

                findProductA.ProductFamilyId.Should().Be(dbProductMoq[0].ProductFamilyId);
                findProductA.ProductGroupId.Should().Be(dbProductMoq[0].ProductGroupId);
                findProductA.ProductTypeId.Should().Be(dbProductMoq[0].ProductTypeId);

                findProductB.ProductFamilyId.Should().Be(dbProductMoq[1].ProductFamilyId);
                findProductB.ProductGroupId.Should().Be(dbProductMoq[1].ProductGroupId);
                findProductB.ProductTypeId.Should().Be(dbProductMoq[1].ProductTypeId);

                findProductC.ProductFamilyId.Should().Be(dbProductMoq[2].ProductFamilyId);
                findProductC.ProductGroupId.Should().Be(dbProductMoq[2].ProductGroupId);
                findProductC.ProductTypeId.Should().Be(dbProductMoq[2].ProductTypeId);

            }         
            var response = await TestClient.PostAsync("/api/Product/Insert", byteContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);                  
        }
    }
}
