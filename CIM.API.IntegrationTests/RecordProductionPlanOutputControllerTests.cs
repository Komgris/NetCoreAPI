using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CIM.API.IntegrationTests {
    public class RecordProductionPlanOutputControllerTests : IntegrationTest {

        [Fact]
        public async Task UpdateMachineProduceCounter_Test()
        {
            var model = new List<MachineProduceCounterModel>();
            model.Add(new MachineProduceCounterModel());
            // Act 
            var content = GetHttpContentForPost(model,"");
            var loadResponse = await TestClient.PostAsync("api/RecordProductionPlanOutput/UpdateMachineProduceCounter", content);
        }
    }
}
