﻿using CIM.Model;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace CIM.API.IntegrationTests
{
    public  class ProductionPlanControllerTest : IntegrationTest
    {

        [Fact]
        public async Task Load_Test()
        {
            var productionPlan = new ProductionPlanModel();

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Load", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/ProductionPlan/{productionPlan}");
            request.Headers.Add("token", AdminToken);
            var getResponse = await TestClient.SendAsync(request);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await getResponse.Content.ReadAsStringAsync()));
            getResult.Data.Id.Should().Equals(productionPlan.Id);
            getResult.Data.RouteId.Should().Equals(productionPlan.RouteId);
            getResult.Data.ProductId.Should().Equals(productionPlan.ProductId);
            getResult.Data.ActualFinish.Should().Equals(productionPlan.ActualFinish);
            getResult.Data.ActualStart.Should().Equals(productionPlan.ActualStart);
        }
    }
}
