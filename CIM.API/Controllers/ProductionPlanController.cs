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

namespace CIM.API.Controllers
{
    [ApiController]
    public class ProductionPlanController : BaseController
    {
        private IProductionPlanService _planService;
        private IActiveProductionPlanService _activePlanService;
        public ProductionPlanController(
            IProductionPlanService planService,
            IActiveProductionPlanService activePlanService
            )
        {
            _planService = planService;
            _activePlanService = activePlanService;
        }

        #region Production plan mng 
        
        [Route("api/[controller]/Compare")]
        [HttpPost]
        public async Task<ProcessReponseModel<List<ProductionPlanModel>>> Compare() {
            var output = new ProcessReponseModel<List<ProductionPlanModel>>();
            try {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("ProductionPlan");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0) {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);
                    }


                    var fromExcel = _planService.ReadImport(fullPath);
                    var result = await _planService.Compare(fromExcel);
                    output.Data = result;
                    output.IsSuccess = true;
                }
                else {
                    output.IsSuccess = false;
                }
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlans")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<ProductionPlanListModel>>> List(int howmany = 10, int page = 1, string keyword = "", int? productId = null, int? routeId = null, string statusIds = null) {
            var output = new ProcessReponseModel<PagingModel<ProductionPlanListModel>>();
            try {
                output.Data = await _planService.List(page, howmany, keyword, productId, routeId, true, statusIds);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }


        [Route("api/[controller]/Import")]
        [HttpPost]
        public async Task<ProcessReponseModel<List<ProductionPlanModel>>> Import([FromBody]List<ProductionPlanModel> data) {
            var output = new ProcessReponseModel<List<ProductionPlanModel>>();
            try {
                output.Data = await _planService.CheckDuplicate(data);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Create([FromBody] ProductionPlanModel data) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                await _planService.Create(data);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Update([FromBody] ProductionPlanModel data) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                await _planService.Update(data);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Delete/{id}")]
        [HttpDelete]
        public async Task<ProcessReponseModel<ProductionPlanListModel>> Delete(string id) {
            var output = new ProcessReponseModel<ProductionPlanListModel>();
            try {
                await _planService.Delete(id);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Load")]
        [HttpGet]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Load(string id) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _planService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };
                output.Data = await _planService.Load(id);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Detail(string id) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                output.Data = await _planService.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        #endregion

        #region Production Process

        [Route("api/ProductionPlanStart")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Start(string planId, int route, int? target) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                await _activePlanService.Start(planId, route, target);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlanFinish")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Finish(ProductionPlanModel model) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                await _activePlanService.Finish(model);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlanPause")]
        [HttpGet]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Pause(string id, int routeId) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _planService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };
                await _activePlanService.Pause(id, routeId);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/ProductionPlanResume")]
        [HttpGet]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Resume(string id, int routeId) {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _planService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };
                await _activePlanService.Resume(id, routeId);
                output.IsSuccess = true;
            }
            catch (Exception ex) {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/FilterLoadProductionPlan")]
        [HttpGet]
        public ProcessReponseModel<object> FilterLoadProductionPlan(int? productId,int? routeId,int?statusId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data =JsonConvert.SerializeObject(_planService.FilterLoadProductionPlan(productId, routeId, statusId), JsonsSetting);
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