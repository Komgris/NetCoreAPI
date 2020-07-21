using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CIM.Model;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Net.Http.Headers;
using CIM.BusinessLogic.Utility;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using CIM.API.HubConfig;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ProductionPlanController : BoardcastController
    {
        private IProductionPlanService _productionPlanService;
        public ProductionPlanController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config,
            IProductionPlanService productionPlanService,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        {
            _productionPlanService = productionPlanService;
        }

        #region Production plan mng 

        [Route("api/[controller]/Compare")]
        [HttpPost]
        public async Task<ProcessReponseModel<List<ProductionPlanModel>>> Compare()
        {
            var output = new ProcessReponseModel<List<ProductionPlanModel>>();
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("ProductionPlan");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var fromExcel = _productionPlanService.ReadImport(fullPath);
                    var result = await _productionPlanService.Compare(fromExcel);
                    output.Data = result;
                    output.IsSuccess = true;
                }
                else
                {
                    output.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlans")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<ProductionPlanListModel>>> List(int howmany = 10, int page = 1, string keyword = "", int? productId = null, int? routeId = null, string statusIds = null)
        {
            var output = new ProcessReponseModel<PagingModel<ProductionPlanListModel>>();
            try
            {
                output.Data = await _productionPlanService.List(page, howmany, keyword, productId, routeId, true, statusIds);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }


        [Route("api/[controller]/Import")]
        [HttpPost]
        public async Task<ProcessReponseModel<List<ProductionPlanModel>>> Import([FromBody]List<ProductionPlanModel> data)
        {
            var output = new ProcessReponseModel<List<ProductionPlanModel>>();
            try
            {
                output.Data = await _productionPlanService.CheckDuplicate(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Create([FromBody] ProductionPlanModel data)
        {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try
            {
                await _productionPlanService.Create(data);
                await _masterDataService.Refresh(Constans.MasterDataType.ProductionPlan);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Update([FromBody] ProductionPlanModel data)
        {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try
            {
                await _productionPlanService.Update(data);
                await _masterDataService.Refresh(Constans.MasterDataType.ProductionPlan);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Delete")]
        [HttpDelete]
        public async Task<ProcessReponseModel<ProductionPlanListModel>> Delete(string id)
        {
            var output = new ProcessReponseModel<ProductionPlanListModel>();
            try
            {
                await _productionPlanService.Delete(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Load")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> Load(string id, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(( await _productionPlanService.Load(id, routeId)), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Detail(string id)
        {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try
            {
                output.Data = await _productionPlanService.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/ListByMonth")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ProductionPlanListModel>>> ListByMonth(int month, int year)
        {
            var output = new ProcessReponseModel<List<ProductionPlanListModel>>();
            try
            {
                output.Data = await _productionPlanService.ListByMonth(month, year);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [Route("api/[controller]/ListByDate")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<ProductionPlanListModel>>> ListByDate(DateTime date, int page, int howmany)
        {
            var output = new ProcessReponseModel<PagingModel<ProductionPlanListModel>>();
            try
            {
                output.Data = await _productionPlanService.ListByDate(date, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }
        #endregion

        #region Production Process

        [Route("api/ProductionPlanStart")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> Start(string planId, int routeId, int? target)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var result = await _activeProductionPlanService.Start(planId, routeId, target);
                output = await HandleResult(result);
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/ProductionPlanFinish")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> Finish(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var result = await _activeProductionPlanService.Finish(planId, routeId);
                await HandleBoardcastingActiveProcess(Constans.BoardcastType.End, planId
                        , new int[] {routeId }, result);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/ProductionPlanPause")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> Pause(string planId, int routeId, int lossLevel3Id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var result = await _activeProductionPlanService.Pause(planId, routeId, lossLevel3Id);
                output = await HandleResult(result);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlanResume")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> Resume(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var result = await _activeProductionPlanService.Resume(planId, routeId);
                output = await HandleResult(result);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/FilterLoadProductionPlan")]
        [HttpGet]
        public ProcessReponseModel<object> FilterLoadProductionPlan(int? productId, int? routeId, int? statusId, string planId = "")
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(_productionPlanService.FilterLoadProductionPlan(productId, routeId, statusId, planId), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlan/GetActiveRoutes")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<int>>> GetActiveRoutes(string planId)
        {
            var output = new ProcessReponseModel<List<int>>();
            try
            {
                output.Data = (await _activeProductionPlanService.GetCached(planId)).ActiveProcesses.Select( x=>x.Key).ToList();
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        private async Task<ProcessReponseModel<object>> HandleResult(ActiveProductionPlanModel model)
        {
            var output = new ProcessReponseModel<object>();
            if (model != null)
            {
                await HandleBoardcastingActiveProcess(Constans.BoardcastType.All, model.ProductionPlanId
                    , model.ActiveProcesses.Where(x=>x.Value.Status != Constans.PRODUCTION_PLAN_STATUS.Finished).Select(o => o.Key).ToArray(), model);

                output.IsSuccess = true;
            }
            return output;
        }

        #endregion
    }
}