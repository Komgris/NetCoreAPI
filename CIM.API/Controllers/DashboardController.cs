﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : BoardcastController {

        public DashboardController(
        IResponseCacheService responseCacheService,
        IHubContext<GlobalHub> hub,
        IReportService service,
        IConfiguration config,
        IActiveProductionPlanService activeProductionPlanService
        ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        { }

        #region Management

        [HttpGet]
        public async Task<string> GetDashboardCached(string channel)
        {
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_DASHBOARD}-{channel}";
            return CacheForBoardcast<BoardcastModel>(await GetCached(channelKey));
        }

        [HttpGet]
        public async Task<string> BoardcastingDashboard(DataFrame dataFrame, BoardcastType updateType, string channel)
        {
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_DASHBOARD}-{channel}";
            var boardcastData = await _service.GenerateBoardcastManagementData(dataFrame, updateType);
            if (boardcastData.Data.Count > 0)
            {
                await HandleBoardcastingManagementData(channelKey, boardcastData);
            }

            return JsonConvert.SerializeObject(boardcastData, JsonsSetting);
        }

        [HttpGet]
        public async Task<string> GetStandardManagementDashboard(DataFrame timeFrame)
        {
            var output = "";
            try
            {
                output = JsonConvert.SerializeObject(await _service.GetManagementDashboard(timeFrame), JsonsSetting);
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }
            return output;
        }

        #endregion

        #region Operation

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionDasboard(string planId, int routeId, int machineId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionDasboard(planId, routeId, machineId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<string> GetActiveBoardcastCached(string productionPlan, int routeId)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var activeProductionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(channelKey);
            if (activeProductionPlan != null && activeProductionPlan.ActiveProcesses.ContainsKey(routeId) && activeProductionPlan.ActiveProcesses[routeId].BoardcastData == null)
            {
                activeProductionPlan.ActiveProcesses[routeId].BoardcastData =
                    await _service.GenerateBoardcastData(BoardcastType.All, productionPlan, routeId);
            }

            return JsonConvert.SerializeObject(activeProductionPlan, JsonsSetting);
        }

        [HttpGet]
        public async Task<string> BoardcastingActiveOperation(BoardcastType updateType, string productionPlan, int routeId)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var activeProductionPlan = await GetCached<ActiveProductionPlanModel>(channelKey);
            if (activeProductionPlan!.ActiveProcesses[routeId] != null)
            {
                return JsonConvert.SerializeObject(
                    await HandleBoardcastingActiveProcess(updateType, productionPlan, new int[] { routeId }, activeProductionPlan));
            }
            return "";
        }

        #endregion

        #region Cim-Oper Production overview

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionSummary(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionSummary(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }

            return output;

        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionPlanInfomation(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionPlanInfomation(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionOperators(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionOperators(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionEvents(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionEvents(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetCapacityUtilisation(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetCapacityUtilisation(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion


        #region Cim-oper mc status

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetActiveMachineInfo(string planId, int routeId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineInfo(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetActiveMachineEvents(string planId, int routeId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineEvents(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineStatusHistory(howMany, page, planId, routeId, machineId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetMachineSpeed(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineSpeed(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionLoss(string planId, int routeId, int lossLv, int? machineId, int? lossId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planId, routeId, lossLv, machineId, lossId, null, null), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

    }
}