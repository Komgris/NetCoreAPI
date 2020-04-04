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
        { //Todo Kom เพิ่ม ม็อคอัพ ยูนีค
            return new List<ProductModel>()
            {
                new ProductModel{ Code="testA",ProductFamily_Id=1,ProductGroup_Id=1,ProductType_Id=3 },
                new ProductModel{ Code="testB",ProductFamily_Id=2,ProductGroup_Id=1,ProductType_Id=4 },
                new ProductModel{ Code="testC",ProductFamily_Id=2,ProductGroup_Id=1,ProductType_Id=3 },
            };
        }

        public static void CompareModelProduct(List<ProductModel> model,ProductModel expect)
        {
            var compareModel = model.First(x => x.Code == expect.Code);
            compareModel.Code.Should().Be(expect.Code);
            compareModel.ProductFamily_Id.Should().Be(expect.ProductFamily_Id);
            compareModel.ProductGroup_Id.Should().Be(expect.ProductGroup_Id);
            compareModel.ProductType_Id.Should().Be(expect.ProductType_Id);
        }
    }

}