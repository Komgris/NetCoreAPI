using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class MachineTestHelper
    {
        public static MachineListModel GetMock(string name = "Sealing1")
        {
            return new MachineListModel()
            {
                Name = name,
                StatusId = 1,
                Status = "Idle",
                MachineTypeId = 2,
                Type = "Sealing",
                IsActive = true,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "api",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };
        }

        public static void CompareModel(MachineListModel model, MachineListModel expected)
        {
            model.Id.Should().Be(expected.Id);
            model.Name.Should().Be(expected.Name);
            model.StatusId.Should().Be(expected.StatusId);
            model.Status.Should().Be(expected.Status);
            model.MachineTypeId.Should().Be(expected.MachineTypeId);
            model.Type.Should().Be(expected.Type);
            model.IsActive.Should().Be(expected.IsActive);
            model.IsDelete.Should().Be(expected.IsDelete);
            model.CreatedAt.Should().Be(expected.CreatedAt);
            model.CreatedBy.Should().Be(expected.CreatedBy);
            model.UpdatedAt.Should().Be(expected.UpdatedAt);
            model.UpdatedBy.Should().Be(expected.UpdatedBy);
        }
    }
}
