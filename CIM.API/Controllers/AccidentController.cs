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
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccidentController : BoardcastController
    {
        private IAccidentService _service;
        ITriggerQueueService _triggerService;

        public AccidentController(
            ITriggerQueueService triggerService,
            IAccidentService service,
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService

            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _hub = hub;
            _service = service;
            _triggerService = triggerService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<ProcessReponseModel<PagingModel<AccidentModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<AccidentModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany);
                output.IsSuccess = true;

            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("End")]
        public async Task<ProcessReponseModel<AccidentModel>> End(int id)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _service.End(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<AccidentModel>> Get(int id)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<AccidentModel>> Create(AccidentModel model)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _service.Create(model);
                //dole dashboard
                _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.HSE);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<AccidentModel>> Update(AccidentModel model)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _service.Update(model);
                //dole dashboard
                _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.HSE);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }


        [HttpDelete]
        public async Task<ProcessReponseModel<AccidentModel>> Delete(int id)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _service.Delete(id);
                //dole dashboard
                _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.HSE);
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
