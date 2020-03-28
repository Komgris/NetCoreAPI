using System;
using System.Collections.Generic;
using System.Text;
using CIM.Model;

namespace CIM.API.IntegrationTests
{
    public static class TestHelper
    {

        public static bool IsDateTimeEqual(DateTime source, DateTime comparison)
        {
            return source.Day == comparison.Day &&
                source.Month == comparison.Month &&
                source.Year == comparison.Year &&
                source.Hour == comparison.Hour &&
                source.Minute == comparison.Minute &&
                source.Second == comparison.Second &&
                source.Day == comparison.Day
                ;
        }

        internal static object GetMock(string code)
        {
            throw new NotImplementedException();
        }

        internal static void CompareModel(MaterialModel responseModel, MaterialModel updateResponseModel)
        {
            throw new NotImplementedException();
        }
    }
}
