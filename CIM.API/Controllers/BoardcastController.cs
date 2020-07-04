using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    [ApiController]
    public class BoardcastController : BaseController {

        public IHubContext<GlobalHub> _hub;
        public IResponseCacheService _responseCacheService;
        public IReportService _service;
        public IConfiguration _config;
        public IActiveProductionPlanService _activeProductionPlanService;

        public BoardcastController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) 
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _service = service;
            _config = config;
            _activeProductionPlanService = activeProductionPlanService;

        }

        #region general


        internal string CacheForBoardcast<T>(string cache)
        {
            var model = JsonConvert.DeserializeObject<T>(cache);
            return JsonConvert.SerializeObject(model, JsonsSetting);
        }

        internal async Task<string> GetCached(string channelKey)
        {
            return await _responseCacheService.GetAsync(channelKey);
        }

        internal async Task<T> GetCached<T>(string channelKey)
        {
            return await _responseCacheService.GetAsTypeAsync<T>(channelKey);
        }

        internal async Task BoardcastClientData(string channel, object data)
        {
            await _hub.Clients.All.SendAsync(channel, JsonConvert.SerializeObject(data, JsonsSetting));
        }

        #endregion


        #region Management

        internal async Task SetBoardcastDataCached(string channelKey, BoardcastModel model)
        {
            var cache = await GetCached<BoardcastModel>(channelKey);
            if (cache == null)
            {
                cache = model;
            }
            else
            {
                foreach (BoardcastDataModel dashboard in model.Data)
                {
                    cache.SetData(dashboard);
                }
            }
            await _responseCacheService.SetAsync(channelKey, cache);
        }

        internal async Task HandleBoardcastingManagementData(string channelKey, BoardcastModel boardcastData)
        {
            if (boardcastData != null)
            {
                await SetBoardcastDataCached(channelKey, boardcastData);
                await BoardcastClientData(channelKey, boardcastData);
            }
        }

        #endregion

        #region Operation
        internal async Task<ActiveProductionPlanModel> HandleBoardcastingActiveProcess(BoardcastType updateType, string productionPlan, int[] routeId, ActiveProductionPlanModel activeModel)
        {
            var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan}";

            foreach (var r in routeId)
            {
                var boardcastData = await _service.GenerateBoardcastData(updateType, productionPlan, r);
                if (boardcastData.Data.Count > 0)
                {
                    await SetBoardcastActiveDataCached(rediskey, r, activeModel, boardcastData);
                    activeModel.ActiveProcesses[r].BoardcastData = boardcastData;
                }
            }
            await BoardcastClientData(channelKey, activeModel);
            return activeModel;
        }

        internal async Task SetBoardcastActiveDataCached(string channelKey, int routeId, ActiveProductionPlanModel activeModel, BoardcastModel model)
        {
            var cache = activeModel.ActiveProcesses[routeId].BoardcastData;
            if (cache is null)
            {
                activeModel.ActiveProcesses[routeId].BoardcastData = model;
            }
            else
            {
                foreach (BoardcastDataModel dashboard in model.Data)
                {
                    cache.SetData(dashboard);
                }
                activeModel.ActiveProcesses[routeId].BoardcastData = cache;
            }

            var readyMachines = await _activeProductionPlanService.ListMachineReady(activeModel.ProductionPlanId);
            foreach (var machine in activeModel.ActiveProcesses[routeId].Route.MachineList)
            {
                machine.Value.IsReady = readyMachines.Any(x => x == machine.Key);
            }
            await _responseCacheService.SetAsync(channelKey, activeModel);
            var alertLimit = _config.GetValue<int>("AlertLimit");
            activeModel.ActiveProcesses[routeId].Alerts = activeModel.ActiveProcesses[routeId].Alerts.OrderByDescending(x => x.CreatedAt)
                .Where(x=>x.LossLevel3Id == Constans.DEFAULT_LOSS_LV3)
                .Take(alertLimit).ToList();

        }

        #endregion


    }
}