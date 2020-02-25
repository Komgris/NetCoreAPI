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
using CIM.API.IntegrationTests.Helper;

namespace CIM.API.IntegrationTests
{
    public class ProductControllerTest : IntegrationTest
    {
        [Fact]
        public async Task InsertProduct_Test()
        {
            // Arrange   
            var dbProductMoq = TestHelper.GetProductList();
            var beforeInsert = await Get();
            var countBeforeInsert = beforeInsert.Count();
         
            await Insert_Get(dbProductMoq);

            var afterInsert = await Get();
            var countAfterInsert = afterInsert.Count();

            var totalList = countAfterInsert - countBeforeInsert;
            totalList.Should().Be(dbProductMoq.Count());

            foreach (var expect in dbProductMoq)
            {
                TestHelper.CompareModelProduct(afterInsert, expect);
            }
        }

        public async Task<List<ProductModel>> Get()
        {
            var getResponse = await TestClient.GetAsync("/api/Product/GetNoPaging");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<List<ProductModel>>((await getResponse.Content.ReadAsStringAsync()));
            return responseModel;
        }

        public async Task<List<ProductModel>> Insert_Get(List<ProductModel> data)
        {
            var content = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var createResponse = await TestClient.PostAsync("/api/Product/Insert", byteContent);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<List<ProductModel>>((await createResponse.Content.ReadAsStringAsync()));
            return responseModel;
        }

        [Fact]
        public async Task EditProduct_Test()
        {
            var dbProductMoq = TestHelper.GetProductList();
            var afterInsert = await Insert_Get(dbProductMoq);

            afterInsert[0].Code = "EditA";
            afterInsert[1].Code = "EditB";
            afterInsert[2].Code = "EditC";

            var content = JsonConvert.SerializeObject(afterInsert);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var createResponse = await TestClient.PostAsync("/api/Product/Edit", byteContent);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var afterEdit = await Get();
            foreach (var expect in afterInsert)
            {
                TestHelper.CompareModelProduct(afterEdit, expect);
            }
        }     
        [Fact]
        public async Task DeleteProduct_Test()
        {
            var dbProductMoq = TestHelper.GetProductList();
            List<ProductModel> moqList = new List<ProductModel>();
            moqList.Add(dbProductMoq[0]);

            var afterInsert = await Insert_Get(moqList);
            
            var deleteResponse = await TestClient.GetAsync("/api/Product/Delete/"+ afterInsert[0].Id);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var afterDelete = await Get();

            var selected = afterDelete.Where(x => x.Id == afterInsert[0].Id);
            selected.Should().BeNullOrEmpty();
        }
    }
}
