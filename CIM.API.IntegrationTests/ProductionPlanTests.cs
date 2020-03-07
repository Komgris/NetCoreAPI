using CIM.API.IntegrationTests.Helper;
using CIM.Model;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CIM.API.IntegrationTests
{
    public class ProductionPlanTests : IntegrationTest
    {
        [Fact]
        public async Task InsertProductionPlan_Test()
        {
            var dbProductionPlanMoq = ProductionPlanTestHelper.GetProductionPlansList();
            var beforeInsert = await Get();
            var countBeforeInsert = beforeInsert.Count();

            await SendInsertRequest(dbProductionPlanMoq);

            var afterInsert = await Get();
            var countAfterInsert = afterInsert.Count();

            var totalList = countAfterInsert - countBeforeInsert;
            totalList.Should().Be(dbProductionPlanMoq.Count());

            foreach (var expect in dbProductionPlanMoq)
            {
                ProductionPlanTestHelper.CompareModelProductionPlan(afterInsert, expect);
            }
        }

        [Fact]
        public async Task DeleteProduct_Test()
        {
            var dbProductMoq = ProductionPlanTestHelper.GetProductionPlansList();
            List<ProductionPlanModel> moqList = new List<ProductionPlanModel>();
            moqList.Add(dbProductMoq[0]);
            await SendInsertRequest(moqList);

            var deleteResponse = await TestClient.DeleteAsync("/api/ProductionPlan/Delete/" + dbProductMoq[0].PlantId);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var afterDelete = await Get();

            var selected = afterDelete.Where(x => x.PlantId == dbProductMoq[0].PlantId);
            selected.Should().BeNullOrEmpty();
        }

        public async Task<List<ProductionPlanModel>> Get()
        {
            var getResponse = await TestClient.GetAsync("/api/ProductionPlan/GetNoPaging");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<List<ProductionPlanModel>>((await getResponse.Content.ReadAsStringAsync()));
            return responseModel;
        }

        public async Task SendInsertRequest(List<ProductionPlanModel> data)
        {
            var content = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var createResponse = await TestClient.PostAsync("/api/ProductionPlan/Insert", byteContent);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
