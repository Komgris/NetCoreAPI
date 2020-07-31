﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public class RecordManufacturingLossController : BoardcastController
    {
        private IRecordManufacturingLossService _recordManufacturingLossService;

        public RecordManufacturingLossController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config,
            IRecordManufacturingLossService recordManufacturingLossService,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        {
            _recordManufacturingLossService = recordManufacturingLossService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<object>> Create(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _recordManufacturingLossService.Create(model);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveMachineInfo, productionPlan.ProductionPlanId
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

        [HttpPost]
        [Route("api/[controller]/End")]
        public async Task<ProcessReponseModel<object>> End(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _recordManufacturingLossService.End(model);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveMachineInfo, productionPlan.ProductionPlanId
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
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _recordManufacturingLossService.Update(model);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveMachineInfo, productionPlan.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    if (model.WasteList.Count > 0)
                    {
                        await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveWaste, productionPlan.ProductionPlanId
                            , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                    }
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetByGuid")]
        public async Task<ProcessReponseModel<RecordManufacturingLossModel>> GetByGuid(Guid guid)
        {
            var output = new ProcessReponseModel<RecordManufacturingLossModel>();
            try
            {
                output.Data = await _recordManufacturingLossService.GetByGuid(guid);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<RecordManufacturingLossModel>>> List(string planId, int? routeId = null, string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<RecordManufacturingLossModel>>();
            try
            {
                output.Data = await _recordManufacturingLossService.List(planId, routeId, keyword, page, howmany);
                output.IsSuccess = true;
            } 
            catch( Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }
    }
}