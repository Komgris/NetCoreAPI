﻿using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using CIM.API.IntegrationTests.Helper;
using System.Linq;
namespace CIM.API.IntegrationTests
{
    public class LossMappingControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GetComponentTypeLossLevel3List_Test()
        {
            // Act
            var response = await TestClient.GetAsync("/api/ComponentTypeLossLevel3/List?componentTypeId=41&page=2&howmany=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var authResult = JsonConvert.DeserializeObject<ProcessReponseModel<PagingModel<ComponentTypeLossLevel3ListModel>>>((await response.Content.ReadAsStringAsync()));
            authResult.IsSuccess.Should().BeTrue();
            authResult.Data.Data[0].ComponentTypeId.Should().Be(41);
        }

        [Fact]
        public async Task GetMachineTypeLossLevel3List_Test()
        {
            // Act
            var response = await TestClient.GetAsync("/api/MachineTypeLossLevel3/List?machineTypeId=16&page=1&howmany=15");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var authResult = JsonConvert.DeserializeObject<ProcessReponseModel<PagingModel<MachineTypeLossLevel3ListModel>>>((await response.Content.ReadAsStringAsync()));
            authResult.IsSuccess.Should().BeTrue();
            authResult.Data.Data[0].MachineTypeId.Should().Be(16);
        }

        [Fact]
        public async Task PostMachineTypeLossLevel3List_Test()
        {
            var token = string.Empty;
            int machineTypeId = 24;

            List<int> lossLevel3IdsTest = new List<int> { 55, 9999 };
            List<int> lossLevel3IdsBackup = new List<int> { };

            var response = await TestClient.GetAsync("/api/MachineTypeLossLevel3/List?machineTypeId=" + machineTypeId + "&page=1&howmany=15");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var authResult = JsonConvert.DeserializeObject<ProcessReponseModel<PagingModel<MachineTypeLossLevel3ListModel>>>((await response.Content.ReadAsStringAsync()));
            authResult.IsSuccess.Should().BeTrue();
            authResult.Data.Data[0].MachineTypeId.Should().Be(machineTypeId);
            for (var i = 0; i < authResult.Data.Data.Count; i++)
            {
                lossLevel3IdsBackup.Add(authResult.Data.Data[i].LossLevel3Id);
            }

            var content = GetHttpContentForPost(lossLevel3IdsTest, token);
            var updateResponse = await TestClient.PostAsync("/api/MachineTypeLossLevel3/Update?machineTypeId=" + machineTypeId, content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseModel = JsonConvert.DeserializeObject<ProcessReponseModel<MachineTypeLossLevel3Model>>((await updateResponse.Content.ReadAsStringAsync()));
            updateResponseModel.IsSuccess.Should().Be(true);

            response = await TestClient.GetAsync("/api/MachineTypeLossLevel3/List?machineTypeId=" + machineTypeId + "&page=1&howmany=15");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            authResult = JsonConvert.DeserializeObject<ProcessReponseModel<PagingModel<MachineTypeLossLevel3ListModel>>>((await response.Content.ReadAsStringAsync()));
            authResult.IsSuccess.Should().BeTrue();

            for (var i = 0; i < authResult.Data.Data.Count; i++)
            {
                authResult.Data.Data[i].LossLevel3Id.Should().Be(lossLevel3IdsBackup[i]);
            }

            content = GetHttpContentForPost(lossLevel3IdsBackup, token);
            updateResponse = await TestClient.PostAsync("/api/MachineTypeLossLevel3/Update?machineTypeId=" + machineTypeId, content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            updateResponseModel = JsonConvert.DeserializeObject<ProcessReponseModel<MachineTypeLossLevel3Model>>((await updateResponse.Content.ReadAsStringAsync()));
            updateResponseModel.IsSuccess.Should().Be(true);
        }
    }
}
