using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.Helper

{
    public static class TestHelper
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
    }

}