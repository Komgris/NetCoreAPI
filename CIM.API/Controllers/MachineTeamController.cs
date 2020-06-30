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
    [ApiController]
    public class MachineTeamController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private IMachineTeamService _service;

        public MachineTeamController(
            IResponseCacheService responseCacheService,
            IMachineTeamService service
        )
        {
            _responseCacheService = responseCacheService;
            _service = service;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<MachineTeamModel>>> List(string keyword = "", int page = 1, int howMany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<MachineTeamModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<MachineTeamModel>> Get(int id)
        {
            var output = new ProcessReponseModel<MachineTeamModel>();
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

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<MachineTeamModel>> Update([FromBody] MachineTeamModel data)
        {
            var output = new ProcessReponseModel<MachineTeamModel>();
            try
            {
                await _service.Update(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<MachineTeamModel>> Create([FromBody] MachineTeamModel data)
        {
            var output = new ProcessReponseModel<MachineTeamModel>();
            try
            {
                await _service.Create(data);
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