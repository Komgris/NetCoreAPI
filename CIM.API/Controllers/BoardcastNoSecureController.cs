﻿using System;
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
    public class BoardcastNoSecureController : BaseNoSecureController
    {

        public IHubContext<GlobalHub> _hub;
        public IResponseCacheService _responseCacheService;
        public IDashboardService _dashboardService;
        public IConfiguration _config;
        public IActiveProductionPlanService _activeProductionPlanService;

        public BoardcastNoSecureController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IDashboardService service,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) 
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _dashboardService = service;
            _config = config;
            _activeProductionPlanService = activeProductionPlanService;
        }

        #region general

        internal string CachedCHKey(DashboardCachedCH cachedCH)
        {
            return $"{Constans.SIGNAL_R_CHANNEL_DASHBOARD}-CachedCH-{cachedCH.ToString()}";//Ex. dashboard-CachedCH-Dole_Custom_Dashboard
        }

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

        internal async Task HandleBoardcastingData(string channelKey, BoardcastModel boardcastData)
        {
            if (boardcastData != null)
            {
                await SetBoardcastDataCached(channelKey, boardcastData);
                await BoardcastClientData(channelKey, boardcastData);
            }
        }

        private async Task SetBoardcastDataCached(string channelKey, BoardcastModel model)
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
            cache.LastUpdate = DateTime.Now;
            await _responseCacheService.SetAsync(channelKey, cache);
        }


        #endregion

        #region Operation

        internal async Task<ActiveProductionPlanModel> HandleBoardcastingActiveProcess(DataTypeGroup updateType, string productionPlan, int[] routeId, ActiveProductionPlanModel activeModel)
        {
            var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan}";

            //generate data for boardcast
            foreach (var r in routeId)
            {
                var boardcastData = await _dashboardService.GenerateBoardcast(updateType, productionPlan, r);
                if (boardcastData.Data.Count > 0)
                {
                    activeModel = await SetBoardcastActiveDataCached(rediskey, r, activeModel, boardcastData);
                }
            }

            await BoardcastClientData(channelKey, activeModel);
            return activeModel;
        }

        private async Task<ActiveProductionPlanModel> SetBoardcastActiveDataCached(string channelKey, int routeId, ActiveProductionPlanModel activeModel, BoardcastModel model)
        {
            var cache = activeModel.ActiveProcesses[routeId].BoardcastData;
            if (cache is null)
            {
                activeModel.ActiveProcesses[routeId].BoardcastData = model;
            }
            else
            {
                //update only new dashboard
                foreach (BoardcastDataModel dashboard in model.Data)
                {
                    cache.SetData(dashboard);
                }
                activeModel.ActiveProcesses[routeId].BoardcastData = cache;
            }

            var recordingMachines = await _activeProductionPlanService.ListMachineLossRecording(activeModel.ProductionPlanId);
            var autorecordingMachines = await _activeProductionPlanService.ListMachineLossAutoRecording(activeModel.ProductionPlanId);
            foreach (var machine in activeModel.ActiveProcesses[routeId].Route.MachineList)
            {
                machine.Value.IsReady = recordingMachines.Contains(machine.Key);
                if (machine.Value.IsReady)
                {
                    machine.Value.IsAutoLossRecord = autorecordingMachines.Contains(machine.Key);
                }
            }

            await _responseCacheService.SetAsync(channelKey, activeModel);
            activeModel.ActiveProcesses[routeId].Alerts = LimitAlert(activeModel.ActiveProcesses[routeId].Alerts);

            return activeModel;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public List<AlertModel> LimitAlert(List<AlertModel> alerts)
        {
            var defaultLoss = _config.GetValue<int>("DefaultLosslv3Id");
            return alerts.OrderByDescending(x => x.CreatedAt)
                .Where(x => x.LossLevel3Id == defaultLoss)
                .ToList();
        }

        #endregion

    }
}