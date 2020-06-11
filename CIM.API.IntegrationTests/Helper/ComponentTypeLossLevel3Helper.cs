using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class ComponentTypeLossLevel3Helper
    {
        public static ComponentTypeLossLevel3Model GetMockLevel3(int id = 300, int componentTypeId = 1, int lossLevel3Id = 1)
        {
            return new ComponentTypeLossLevel3Model()
            {
                Id = id,
                ComponentTypeId = componentTypeId,
                LossLevel3Id = lossLevel3Id,
            };
        }

        public static void CompareLevel3Model(ComponentTypeLossLevel3Model model, ComponentTypeLossLevel3Model expected)
        {
            model.Id.Should().Be(expected.Id);
            model.ComponentTypeId.Should().Be(expected.ComponentTypeId);
            model.LossLevel3Id.Should().Be(expected.LossLevel3Id);
        }
    }
}
