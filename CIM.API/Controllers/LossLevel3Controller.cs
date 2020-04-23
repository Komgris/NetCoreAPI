using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.Json;
using CIM.API.HubConfig;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace CIM.API.Controllers
{
    [ApiController]
    public class LossLevel3Controller : ControllerBase
    {

        private ILossLevel3Service _service;
        public LossLevel3Controller(
            ILossLevel3Service service
        )
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<LossLevel3ViewModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<LossLevel3ViewModel>>();
            try
            {
                // todo
                output.Data = await _service.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<LossLevel3Model> Create([FromBody]LossLevel3EditableModel model)
        {
            try
            {
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                return await _service.Create(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<LossLevel3Model> Update([FromBody]LossLevel3EditableModel model)
        {
            try
            {
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };
                return await _service.Update(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
