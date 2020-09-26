using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class MaterialTestHelper
    {
        public static MaterialModel GetMock(string code= "TESTCODE")
        {
           return new MaterialModel()
            {
                Code = code,
                Description = "Test description",
                ProductCategory = "Ingredient",
                ICSGroup = "Ingredient",
                MaterialGroup = "WHITE GRAPE JUICE",
                UOM = "KG",
                BHTPerUnit = 999,
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
            model.ICSGroup.Should().Be(expected.ICSGroup);
            model.MaterialGroup.Should().Be(expected.MaterialGroup);
            model.UOM.Should().Be(expected.UOM);
            model.BHTPerUnit.Should().Be(expected.BHTPerUnit);
            model.IsActive.Should().Be(expected.IsActive);
            model.IsDelete.Should().Be(expected.IsDelete);
            model.CreatedAt.Should().Be(expected.CreatedAt);
            model.CreatedBy.Should().Be(expected.CreatedBy);
            model.UpdatedAt.Should().Be(expected.UpdatedAt);
            model.UpdatedBy.Should().Be(expected.UpdatedBy);
        }
    }
}
