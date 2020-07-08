using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System.Collections.Generic;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ComponentTypeLossLevel3Controller : ControllerBase
    {
        private IComponentTypeLossLevel3Service _service;
        public ComponentTypeLossLevel3Controller(
            IComponentTypeLossLevel3Service service
        )
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<ComponentTypeLossLevel3ListModel>>> List(int? componentTypeId, int? lossLevel3Id, int page = 1, int howmany = 15)
        {
            var output = new ProcessReponseModel<PagingModel<ComponentTypeLossLevel3ListModel>>();
            try
            {
                output.Data = await _service.List(componentTypeId, lossLevel3Id, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> Update([FromBody] List<int> lossLevel3Ids, int componentTypeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Update(lossLevel3Ids, componentTypeId);

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