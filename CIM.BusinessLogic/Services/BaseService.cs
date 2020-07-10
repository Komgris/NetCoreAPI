using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CIM.BusinessLogic.Services
{
    public class BaseService
    {
        public string CurrentUserId { get; set; } = "SomeAdmin";
        public string CurrentLanguage { get; set; } = "en";
        public static string ImagePath { get; set; }

        public CurrentUserModel CurrentUser { get; set; } = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" }; //testadmin
        
        public PagingModel<T> ToPagingModel<T>(List<T> data, int total, int page, int howmany)
        where T : new() {
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