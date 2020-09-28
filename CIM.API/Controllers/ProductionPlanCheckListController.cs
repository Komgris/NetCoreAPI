using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ProductionPlanCheckListController : BaseController
    {
        private IProductionPlanCheckListService _service;
        public ProductionPlanCheckListController(
            IHubContext<GlobalHub> hub,
            IProductionPlanCheckListService service,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _service = service;
            _masterDataService = masterDataService;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<List<ProductionPlanCheckListModel>>> List(int machineTypeId,int checkListTypeId)
        {
            var output = new ProcessReponseModel<List<ProductionPlanCheckListModel>>();
            try
            {
                output.Data = await _service.List(machineTypeId, checkListTypeId);
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