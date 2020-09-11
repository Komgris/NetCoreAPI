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
    public class LossLevel3Controller : BaseController
    {

        private ILossLevel3Service _service;
        public LossLevel3Controller(
            IHubContext<GlobalHub> hub,
            ILossLevel3Service service,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _service = service;
            _masterDataService = masterDataService;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<LossLevel3ListModel>>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true, int? lossLevel2Id = null)
        {
            var output = new ProcessReponseModel<PagingModel<LossLevel3ListModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany, isActive, lossLevel2Id);
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
        public async Task<ProcessReponseModel<object>> Create([FromBody] LossLevel3Model model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await _service.Create(model);
                await RefreshMasterData(Constans.MasterDataType.LossLevel3s);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update([FromBody] LossLevel3Model model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await _service.Update(model);
                await RefreshMasterData(Constans.MasterDataType.LossLevel3s);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<LossLevel3Model>> Get(int id)
        {
            var output = new ProcessReponseModel<LossLevel3Model>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }
    }
}
