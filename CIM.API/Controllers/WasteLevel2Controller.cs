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
    public class WasteLevel2Controller : BaseController
    {

        private IWasteLevel2Service _service;
        public WasteLevel2Controller(
            IHubContext<GlobalHub> hub,
            IWasteLevel2Service service,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _service = service;
            _masterDataService = masterDataService;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<WasteLevel2Model>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<WasteLevel2Model>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive);
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
        public async Task<ProcessReponseModel<object>> Create([FromBody] WasteLevel2Model model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await _service.Create(model);
                await  RefreshMasterData(Constans.MasterDataType.WastesLevel2);
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
        public async Task<ProcessReponseModel<object>> Update([FromBody] WasteLevel2Model model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await _service.Update(model);
                await  RefreshMasterData(Constans.MasterDataType.WastesLevel2);
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
        public async Task<ProcessReponseModel<WasteLevel2Model>> Get(int id)
        {
            var output = new ProcessReponseModel<WasteLevel2Model>();
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
