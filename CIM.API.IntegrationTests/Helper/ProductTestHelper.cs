using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIM.API.IntegrationTests.Helper

{
    public static class ProductTestHelper
    {
        public static ProductModel GetProduct(string code = "Sealing1")
        { //Todo Kom เพิ่ม ม็อคอัพ ยูนีค
            return new ProductModel()
            {
                Code = code,
                Description = "Test description",
                BriteItemPerUpcitem = "Ingredient",
                ProductFamily_Id = 1,
                ProductGroup_Id = 2,
                ProductType_Id = 3,
                PackingMedium = "pack",
                NetWeight = 10,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "api",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };
        }

        public static void CompareModelProduct(ProductModel model,ProductModel expected)
        {
            model.Id.Should().Be(expected.Id);
            model.Code.Should().Be(expected.Code);
            model.Description.Should().Be(expected.Description);
            model.BriteItemPerUpcitem.Should().Be(expected.BriteItemPerUpcitem);
            model.ProductFamily_Id.Should().Be(expected.ProductFamily_Id);
            model.ProductGroup_Id.Should().Be(expected.ProductGroup_Id);
            model.ProductType_Id.Should().Be(expected.ProductType_Id);
            model.NetWeight.Should().Be(expected.NetWeight);
            model.IsDelete.Should().Be(expected.IsDelete);
            model.CreatedAt.Should().Be(expected.CreatedAt);
            model.CreatedBy.Should().Be(expected.CreatedBy);
            model.UpdatedAt.Should().Be(expected.UpdatedAt);
            model.UpdatedBy.Should().Be(expected.UpdatedBy);
        }
    }

}