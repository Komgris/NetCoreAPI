using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Text.Json;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CIM.API.HubConfig;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers
{
    [ApiController]
    public class ProductController : BaseController
    {
        private IProductService _service;
        private IUtilitiesService _utilitiesService;

        public ProductController(
            IHubContext<GlobalHub> hub,
            IProductService service,
            IUtilitiesService utilitiesService,
            IMasterDataService masterDataService
            )
        {
            _hub = hub;
            _service = service;
            _utilitiesService = utilitiesService;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductModel>> Create([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<ProductModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<ProductModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "product", false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"product/{list.Image}";
                }
                await _service.Create(list);
                await RefreshMasterData(Constans.MasterDataType.Products);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/[controller]/Delete")]
        [HttpDelete]
        public async Task<ProcessReponseModel<object>> Delete(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Delete(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<ProductModel>> Get(int id)
        {
            var output = new ProcessReponseModel<ProductModel>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<ProductModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true, int? processTypeId = null)
        {
            var output = new ProcessReponseModel<PagingModel<ProductModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive, processTypeId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpPut]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var list = JsonConvert.DeserializeObject<ProductModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "product", false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"product/{list.Image}";
                }
                await _service.Update(list);
                await RefreshMasterData(Constans.MasterDataType.Products);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }
    }
}
