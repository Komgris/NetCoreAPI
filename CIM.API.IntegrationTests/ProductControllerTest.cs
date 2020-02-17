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

namespace CIM.API.IntegrationTests
{
    public class ProductControllerTest : IntegrationTest
    {
        [Fact]
        public async Task InsertProduct_Test()
        {
            var dbProductMoq = new List<ProductModel>()
            {
                new ProductModel{ Code="test",ProductFamilyId=3,ProductGroupId=3,ProductTypeId=3 },
                new ProductModel{ Code="test",ProductFamilyId=3,ProductGroupId=3,ProductTypeId=3 },
                new ProductModel{ Code="test",ProductFamilyId=3,ProductGroupId=3,ProductTypeId=3 },
            };
            var content = JsonConvert.SerializeObject(dbProductMoq);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await TestClient.PostAsync("/api/Product/Insert", byteContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
