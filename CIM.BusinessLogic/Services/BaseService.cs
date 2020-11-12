using CIM.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace CIM.BusinessLogic.Services
{
    public class BaseService
    {
        public static string ImagePath { get; set; }
        public static string DocPath { get; set; }
        public static ExcelMappingModel ExcelMapping { get; set; }
        public static string CurrentUserId { get; set; }
        public static bool IsVerifyTokenPass { get; set; }
        public static Dictionary<string, ActiveProductionPlan3MModel> baseListActive { get; set; } = new Dictionary<string, ActiveProductionPlan3MModel>();
        public static Dictionary<int, ActiveMachine3MModel> baseListMachine { get; set; } = new Dictionary<int, ActiveMachine3MModel>();

        public JsonSerializerSettings JsonsSetting { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public CurrentUserModel CurrentUser
        {
            get => new CurrentUserModel { UserId = CurrentUserId, IsValid = IsVerifyTokenPass };
        }

        public PagingModel<T> ToPagingModel<T>(List<T> data, int total, int page, int howmany)
        where T : new()
        {
            var output = new PagingModel<T>();
            howmany = howmany <= 0 ? 1000000 : howmany;
            output.Total = total;
            output.HowMany = howmany;
            output.Page = page;
            output.NextPage = page + 1;
            output.PreviousPage = page - 1;
            output.PreviousPage = output.PreviousPage < 0 ? 0 : output.PreviousPage;
            var lastPage = Convert.ToInt32(Math.Ceiling((output.Total / Convert.ToDouble(howmany))));
            output.NextPage = output.NextPage > lastPage ? lastPage : output.NextPage;
            var skip = (page - 1) * howmany;
            output.Data = data;
            return output;
        }

    }
}