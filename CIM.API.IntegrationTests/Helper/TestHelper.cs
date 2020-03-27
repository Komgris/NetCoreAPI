using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class TestHelper
    {
        public static MaterialModel GetMock(string code= "TESTCODE")
        {
           return new MaterialModel()
            {
                Code = code,
                Description = "Test description",
                ProductCategory = "Ingredient",
                Icsgroup = "Ingredient",
                MaterialGroup = "WHITE GRAPE JUICE",
                Uom = "KG",
                BhtperUnit = 999,
                IsActive = true,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "api",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };

        }

        public static void CompareModel(MaterialModel model,MaterialModel expected)
        {
            model.Code.Should().Be(expected.Code);
            model.Description.Should().Be(expected.Description);
            model.ProductCategory.Should().Be(expected.ProductCategory);
            model.Icsgroup.Should().Be(expected.Icsgroup);
            model.MaterialGroup.Should().Be(expected.MaterialGroup);
            model.Uom.Should().Be(expected.Uom);
            model.BhtperUnit.Should().Be(expected.BhtperUnit);
            model.IsActive.Should().Be(expected.IsActive);
            model.IsDelete.Should().Be(expected.IsDelete);
            model.CreatedAt.Should().Be(expected.CreatedAt);
            model.CreatedBy.Should().Be(expected.CreatedBy);
            model.UpdatedAt.Should().Be(expected.UpdatedAt);
            model.UpdatedBy.Should().Be(expected.UpdatedBy);
        }
    }
}
