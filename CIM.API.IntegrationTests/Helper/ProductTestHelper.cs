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
        public static List<ProductModel> GetProductList()
        {
            return new List<ProductModel>()
            {
                new ProductModel{ Code="testA",ProductFamilyId=1,ProductGroupId=1,ProductTypeId=3 },
                new ProductModel{ Code="testB",ProductFamilyId=2,ProductGroupId=1,ProductTypeId=4 },
                new ProductModel{ Code="testC",ProductFamilyId=2,ProductGroupId=1,ProductTypeId=3 },
            };
        }

        public static void CompareModelProduct(List<ProductModel> model,ProductModel expect)
        {
            var compareList = model.First(x => x.Code == expect.Code);
            compareList.Code.Should().Be(expect.Code);
            compareList.ProductFamilyId.Should().Be(expect.ProductFamilyId);
            compareList.ProductGroupId.Should().Be(expect.ProductGroupId);
            compareList.ProductTypeId.Should().Be(expect.ProductTypeId);
        }
    }

}