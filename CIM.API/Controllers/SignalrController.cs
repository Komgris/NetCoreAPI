using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SignalrController : BoardcastController
    {

        public SignalrController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
        ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">transfer-message, command-channel</param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Send(string channel, string data)
        {
            await _hub.Clients.All.SendAsync(channel, data);
            return "OK";
        }

        //curl -X GET "https://localhost:44365/api/Signalr/SendChartData?channel=dashboard-CachedCH-Dole_Custom_Dashboard&procedure=sp_get_production_target_output&db=CIMDatabaseDashboard&chartId=OutputChart" -H "accept: text/plain"
        [HttpGet]
        public async Task<string> SendChartData(string channel, string procedure, string db, string chartId, DateTime? datestamp = null)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@date",datestamp}
            };
            var chartData = _dashboardService.GetChartData(paramsList, procedure, db);
            var chart = await Task.Run(() => JsonConvert.SerializeObject(chartData, JsonsSetting));

            var output = new ChartModel {
                Name = chartId,
                DataString = chart,
            };

            await _hub.Clients.All.SendAsync(channel, JsonConvert.SerializeObject(output, JsonsSetting));
            return "OK";
        }


        [HttpGet]
        public async Task<string> UpdatePart(string channel, string chartId, string data)
        {
            
            var output = new ChartModel
            {
                Name = chartId,
                DataString = data,
            };

            await _hub.Clients.All.SendAsync(channel, JsonConvert.SerializeObject(output, JsonsSetting));
            return "OK";
        }
    }

    public class ChartModel
    {
        public string DataString { get; set; }
        public string Name { get; set; }
    }
}
