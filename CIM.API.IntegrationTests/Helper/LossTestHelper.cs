using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class LossTestHelper
    {
        public static LossLevel1Model GetMockLevel3(int id = 300,string name = "test")
        {
            return new LossLevel1Model()
            {
                Id = id,
                Name = name,
                Description = "Test Description",
                IsActive = false,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "Tester",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "Tester",
            };
        }

        public static void CompareLevel3Model(LossLevel3Model model, LossLevel3Model expected)
        {
            model.Id.Should().Be(expected.Id);
            model.Name.Should().Be(expected.Name);
            model.Description.Should().Be(expected.Description);
            model.IsActive.Should().Be(expected.IsActive);
            model.IsDelete.Should().Be(expected.IsDelete);
            model.CreatedAt.Should().Be(expected.CreatedAt);
            model.CreatedBy.Should().Be(expected.CreatedBy);
            model.UpdatedAt.Should().Be(expected.UpdatedAt);
            model.UpdatedBy.Should().Be(expected.UpdatedBy);
        }
    }
}
