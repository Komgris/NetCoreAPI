using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WasteController : BaseController
    {
        private IRecordProductionPlanWasteService _recordProductionPlanWasteService;

        public WasteController(
            IRecordProductionPlanWasteService recordProductionPlanWasteService
            )
        {
            _recordProductionPlanWasteService = recordProductionPlanWasteService;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Create(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _recordProductionPlanWasteService.Create(model);
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
