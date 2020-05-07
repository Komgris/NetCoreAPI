using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class MachineTypeLossLevel3Helper
    {
        public static MachineTypeLossLevel3Model GetMockLevel3(int id = 300, int machineTypeId = 1, int lossLevel3Id = 1)
        {
            return new MachineTypeLossLevel3Model()
            {
                Id = id,
                MachineTypeId = machineTypeId,
                LossLevel3Id = lossLevel3Id,
            };
        }

        public static void CompareLevel3Model(MachineTypeLossLevel3Model model, MachineTypeLossLevel3Model expected)
        {
            model.Id.Should().Be(expected.Id);
            model.MachineTypeId.Should().Be(expected.MachineTypeId);
            model.LossLevel3Id.Should().Be(expected.LossLevel3Id);
        }
    }
}
