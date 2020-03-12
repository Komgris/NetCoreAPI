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

        public static void CompareModel(MaterialModel model,MaterialModel expect)
        {
            model.Code.Should().Be(expect.Code);
            model.Description.Should().Be(expect.Description);
            model.ProductCategory.Should().Be(expect.ProductCategory);
            model.Icsgroup.Should().Be(expect.Icsgroup);
            model.MaterialGroup.Should().Be(expect.MaterialGroup);
            model.Uom.Should().Be(expect.Uom);
            model.BhtperUnit.Should().Be(expect.BhtperUnit);
            model.IsActive.Should().Be(expect.IsActive);
            model.IsDelete.Should().Be(expect.IsDelete);
            model.CreatedAt.Should().Be(expect.CreatedAt);
            model.CreatedBy.Should().Be(expect.CreatedBy);
            model.UpdatedAt.Should().Be(expect.UpdatedAt);
            model.UpdatedBy.Should().Be(expect.UpdatedBy);
        }
    }
}
