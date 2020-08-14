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

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WasteController : BoardcastController
    {
        private IRecordProductionPlanWasteService _recordProductionPlanWasteService;

        public WasteController(
            IRecordProductionPlanWasteService recordProductionPlanWasteService,
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config,
            IRecordManufacturingLossService recordManufacturingLossService,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        {
            _recordProductionPlanWasteService = recordProductionPlanWasteService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<ProcessReponseModel<PagingModel<RecordProductionPlanWasteModel>>> List(string planId, int? routeId = null, string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<RecordProductionPlanWasteModel>>();
            try
            {
                output.Data = await _recordProductionPlanWasteService.List(planId, routeId, keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Create(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _recordProductionPlanWasteService.Create(model);
                var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.ProductionPlanId}";
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveWaste, model.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                }

                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Update(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                await _recordProductionPlanWasteService.Update(model);
                var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.ProductionPlanId}";
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveWaste, model.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                }

                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpDelete]
        public async Task<ProcessReponseModel<object>> Delete(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var dbModel = await _recordProductionPlanWasteService.Get(id);
                await _recordProductionPlanWasteService.Delete(id);
                var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{dbModel.ProductionPlanId}";
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveWaste, dbModel.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                }
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Get(int id)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _recordProductionPlanWasteService.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpPost]
        [Route("NonePrimeCreate")]
        public async Task<ProcessReponseModel<object>> NonePrimeCreate(List<RecordProductionPlanWasteNonePrimeModel> models)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _recordProductionPlanWasteService.NonePrimeCreate(models);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("RecordNonePrimeList")]
        public async Task<ProcessReponseModel<object>> RecordNonePrimeList(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _recordProductionPlanWasteService.RecordNonePrimeList(planId, routeId), JsonsFormatting); 
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }
    }
}
