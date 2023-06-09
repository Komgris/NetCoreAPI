﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
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

        internal async Task HandleBoardcastingData(string channelKey, ProductionDataModel boardcastData)
        {
            if (boardcastData != null)
            {
                await SetBoardcastDataCached(channelKey, boardcastData);
                await BoardcastClientData(channelKey, boardcastData);
            }
        }

        private async Task SetBoardcastDataCached(string channelKey, ProductionDataModel model)
        {
            var cache = await GetCached<ProductionDataModel>(channelKey);
            if (cache == null)
            {
                cache = model;
            }
            else
            {
                foreach (UnitDataModel dashboard in model.UnitData)
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
                if (boardcastData.UnitData.Count > 0)
                {
                    activeModel = await SetBoardcastActiveDataCached(rediskey, r, activeModel, boardcastData);
                }
            }

            await BoardcastClientData(channelKey, activeModel);
            return activeModel;
        }

        internal async Task<ActiveProductionPlan3MModel> HandleBoardcastingActiveProcess3M(DataTypeGroup updateType, string productionPlan, int machineId, ActiveProductionPlan3MModel activeModel)
        {
            var rediskey = $"{RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var channelKey = $"{SIGNAL_R_CHANNEL_PRODUCTION_PLAN}:{productionPlan}";

            //generate data for boardcast
            var boardcastData = await _dashboardService.GenerateBoardcast(updateType, productionPlan, machineId);
            if (boardcastData.UnitData.Count > 0)
            {
                activeModel = await SetBoardcastActiveDataCached3M(rediskey, machineId, activeModel, boardcastData);

                var kpi = boardcastData.UnitData.Where(x => x.Name == "KPI").FirstOrDefault();
                if (kpi.IsSuccess)
                {
                    var jsonData = JsonConvert.DeserializeObject<List<KPI>>(kpi.JsonData);
                    var productionInfo = _responseCacheService.GetProductionInfo() ?? new ProductionInfoModel();
                    productionInfo.MachineInfoList[machineId].OEE = jsonData[0].OEE;
                    productionInfo.MachineInfoList[machineId].Performance = jsonData[0].Performance;
                    productionInfo.MachineInfoList[machineId].Availability = jsonData[0].Availability;
                    productionInfo.MachineInfoList[machineId].Quality = jsonData[0].Quality;
                    await _responseCacheService.SetProductionInfo(productionInfo);
                }
            }

            await BoardcastClientData(channelKey, activeModel);
            return activeModel;
        }

        internal async Task<object> HandleBoardcastingActiveMachine3M(int machineId)
        {
            var activeMachines = _responseCacheService.GetActiveMachine(machineId);
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL.CHANNEL_MACHINE}:{machineId}";

            ////generate data for boardcast
            //foreach (var m in machineId)
            //{
            //    var boardcastData = await _dashboardService.GenerateBoardcast(updateType, productionPlan, m);
            //    if (boardcastData.UnitData.Count > 0)
            //    {
            //        activeModel = await SetBoardcastActiveDataCached3M(rediskey, m, activeModel, boardcastData);
            //    }
            //}

            await BoardcastClientData(channelKey, activeMachines);
            activeMachines.Alerts.Clear();
            return activeMachines;
        }

        private async Task<ActiveProductionPlanModel> SetBoardcastActiveDataCached(string channelKey, int routeId, ActiveProductionPlanModel activeModel, ProductionDataModel model)
        {
            var cache = activeModel.ActiveProcesses[routeId].BoardcastData;
            if (cache is null)
            {
                activeModel.ActiveProcesses[routeId].BoardcastData = model;
            }
            else
            {
                //update only new dashboard
                foreach (UnitDataModel dashboard in model.UnitData)
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

        private async Task<ActiveProductionPlan3MModel> SetBoardcastActiveDataCached3M(string channelKey, int machineId, ActiveProductionPlan3MModel activeModel, ProductionDataModel model)
        {
            var cacheProductionData = activeModel.ProductionData;
            if (cacheProductionData is null)
            {
                activeModel.ProductionData = model;
            }
            else
            {
                //update only new dashboard
                foreach (UnitDataModel dashboard in model.UnitData)
                {
                    cacheProductionData.SetData(dashboard);
                }
                activeModel.ProductionData = cacheProductionData;
            }

            //var recordingMachines = await _activeProductionPlanService.ListMachineLossRecording(activeModel.ProductionPlanId);
            //var autorecordingMachines = await _activeProductionPlanService.ListMachineLossAutoRecording(activeModel.ProductionPlanId);

            var cacheMachine = _responseCacheService.GetActiveMachine(machineId);
            //cacheMachine.LossRecording = recordingMachines.Contains(machineId);
            //if (cacheMachine.LossRecording)
            //{
            //    cacheMachine.IsAutoRecord = autorecordingMachines.Contains(machineId);
            //}

            await _responseCacheService.SetActivePlan(activeModel);
            await _responseCacheService.SetActiveMachine(cacheMachine);
            cacheMachine.Alerts = LimitAlert(cacheMachine.Alerts);

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