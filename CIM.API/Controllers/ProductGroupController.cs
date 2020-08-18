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
    public class ProductGroupController : BaseController
    {
        private IProductGroupService _service;
        public ProductGroupController(
            IProductGroupService service,
            IMasterDataService masterDataService
        )
        {
            _service = service;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<ProductGroupModel>>> List(string keyword = "", int page = 1, int howMany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<ProductGroupModel>>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                output.Data = await _service.List(keyword, page, howMany, isActive);
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
        public async Task<ProcessReponseModel<ProductGroupModel>> Create([FromBody] ProductGroupModel data)
        {
            var output = new ProcessReponseModel<ProductGroupModel>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.Create(data);
                await _masterDataService.Refresh(Constans.MasterDataType.ProductGroup);
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
        public async Task<ProcessReponseModel<ProductGroupModel>> Get(int id)
        {
            var output = new ProcessReponseModel<ProductGroupModel>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

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
        public async Task<ProcessReponseModel<ProductGroupModel>> Update([FromBody] ProductGroupModel data)
        {
            var output = new ProcessReponseModel<ProductGroupModel>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.Update(data);
                await _masterDataService.Refresh(Constans.MasterDataType.ProductGroup);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Delete")]
        [HttpDelete]
        public async Task<ProcessReponseModel<ProductGroupModel>> Delete(int id)
        {
            var output = new ProcessReponseModel<ProductGroupModel>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.Delete(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/InsertMappingByProductGroup")]
        [HttpPost]
        public async Task<ProcessReponseModel<RouteProductGroupModel>> InsertMappingByMachineId([FromBody] List<RouteProductGroupModel> data)
        {
            var output = new ProcessReponseModel<RouteProductGroupModel>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.InsertMappingRouteProductGroup(data);
                await _masterDataService.Refresh(Constans.MasterDataType.Products);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/ListRoutByProductGroup")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<RouteProductGroupModel>>> GetByMachineType(int productGroupId)
        {
            var output = new ProcessReponseModel<List<RouteProductGroupModel>>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                output.Data = await _service.ListRouteByProductGroup(productGroupId);
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