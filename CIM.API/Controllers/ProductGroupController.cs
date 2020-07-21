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
        private IProductGroupService _productGroupService;
        public ProductGroupController(
            IProductGroupService productGroupService
        )
        {
            _productGroupService = productGroupService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<ProductGroupModel>>> List(string keyword = "", int page = 1, int howMany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<ProductGroupModel>>();
            try
            {
                output.Data = await _productGroupService.List(keyword, page, howMany, isActive);
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
                await _productGroupService.Create(data);
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
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _productGroupService.Get(id);
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
                await _productGroupService.Update(data);
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
                await _productGroupService.Delete(id);
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
                await _productGroupService.InsertMappingRouteProductGroup(data);
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
                output.Data = await _productGroupService.ListRouteByProductGroup(productGroupId);
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