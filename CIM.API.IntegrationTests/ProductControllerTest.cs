using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using CIM.DAL.Interfaces;
using Moq;
using CIM.BusinessLogic.Services;

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

            var dbProductMoq = new List<ProductModel>()
            {
                new ProductModel{ Code="testA",ProductFamilyId=1,ProductGroupId=1,ProductTypeId=3 },
                new ProductModel{ Code="testB",ProductFamilyId=2,ProductGroupId=1,ProductTypeId=4 },
                new ProductModel{ Code="testC",ProductFamilyId=2,ProductGroupId=1,ProductTypeId=3 },
            };
            var content = JsonConvert.SerializeObject(dbProductMoq);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await TestClient.PostAsync("/api/Product/Insert", byteContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var outputDb1 = service.List("testA");
            var outputDb2 = service.List("testB");
            var outputDb3 = service.List("testC");

            outputDb1.Should().NotBeNull();
            outputDb2.Should().NotBeNull();
            outputDb3.Should().NotBeNull();

        }
    }
}
