using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [ApiController]
    public class MaterialController : BaseController
    {
        private IMaterialService _service;
        private IUtilitiesService _utilitiesService;
        public MaterialController(
            IHubContext<GlobalHub> hub,
            IMaterialService service,
            IUtilitiesService utilitiesService,
            IMasterDataService masterDataService)
        {
            _hub = hub;
            _service = service;
            _utilitiesService = utilitiesService;
            _masterDataService = masterDataService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<MaterialModel>> Create([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<MaterialModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<MaterialModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "material", false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"material/{list.Image}";
                }
                output.Data = await _service.Create(list);
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
        public async Task<ProcessReponseModel<MaterialModel>> Update([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<MaterialModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<MaterialModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "material", false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"material/{list.Image}";
                }
                output.Data = await _service.Update(list);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<MaterialModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<MaterialModel>>();
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

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<MaterialModel>> Get(int id)
        {
            var output = new ProcessReponseModel<MaterialModel>();
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

        [Route("api/[controller]/ListByProduct")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ProductMaterialModel>>> ListByProduct(int productId)
        {
            var output = new ProcessReponseModel<List<ProductMaterialModel>>();
            try
            {
                output.Data = await _service.ListMaterialByProduct(productId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/[controller]/InsertByProduct")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductMaterialModel>> InsertByProduct([FromBody] List<ProductMaterialModel> data)
        {
            var output = new ProcessReponseModel<ProductMaterialModel>();
            try
            {
                await _service.InsertByProduct(data);
                await RefreshMasterData(Constans.MasterDataType.Products);
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