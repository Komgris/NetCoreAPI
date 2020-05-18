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

namespace CIM.API.Controllers
{
    [ApiController]
    public class ProductionPlanController : BaseController
    {
        private IHubContext<MachineHub> _hub;
        private IProductionPlanService _productionPlanService;
        private IActiveProductionPlanService _activeProductionPlanService;
        public ProductionPlanController(
            IHubContext<MachineHub> hub,
            IProductionPlanService productionPlanService,
            IActiveProductionPlanService activeProductionPlanService
            )
        {
            _hub = hub;
            _productionPlanService = productionPlanService;
            _activeProductionPlanService = activeProductionPlanService;
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
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
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
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Delete/{id}")]
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
        public async Task<ProcessReponseModel<ProductionPlanModel>> Load(string id, int routeId)
        {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try
            {
                output.Data = await _productionPlanService.Load(id, routeId);
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
                output = HandleResult(result);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
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
                output = HandleResult(result);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
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
                output = HandleResult(result);
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
                output = HandleResult(result);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/FilterLoadProductionPlan")]
        [HttpGet]
        public ProcessReponseModel<object> FilterLoadProductionPlan(int? productId, int? routeId, int? statusId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(_productionPlanService.FilterLoadProductionPlan(productId, routeId, statusId), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        private ProcessReponseModel<object> HandleResult(ActiveProductionPlanModel model)
        {
            var output = new ProcessReponseModel<object>();
            if (output.Data != null)
            {
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{model.ProductionPlanId}";
                var dataString = JsonConvert.SerializeObject(model, JsonsSetting);
                _hub.Clients.All.SendAsync(channelKey, dataString);
                output.Data = dataString;
                output.IsSuccess = true;
            }
            return output;
        }

        #endregion
    }
}