using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    //[MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private IMaterialService _service;
        public MaterialController(IMaterialService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<MaterialModel> Create([FromBody]MaterialModel model)
        {
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                //_service.CurrentUser = currentUser;
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
        public async Task<MaterialModel> Update([FromBody]MaterialModel model)
        {
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                //_service.CurrentUser = currentUser;
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                return await _service.Update(model);
            }
            catch (Exception e)
            {
                throw e;
            }
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
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<MaterialModel> Get(int id)
        {
            try
            {
                return await _service.Get(id);
            }
            catch (Exception e)
            {
                throw e;
            }
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
                output.Message = ex.Message;
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