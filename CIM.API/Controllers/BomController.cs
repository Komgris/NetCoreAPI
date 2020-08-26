using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.API.Controllers
{
    [ApiController]
    public class BomController : BaseController
    {
        private IBomService _service;
        public BomController(
            IBomService service
        ) 
        {
            _service = service;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<MaterialGroupModel>>> List(string keyword = "", int page = 1, int howMany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<MaterialGroupModel>>();
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

        [Route("api/[controller]/ListMaterial")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<MaterialGroupMaterialModel>>> ListMaterial(int bomId)
        {
            var output = new ProcessReponseModel<List<MaterialGroupMaterialModel>>();
            try
            {
                output.Data = await _service.ListBomMapping(bomId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/InsertMapping")]
        [HttpPost]
        public async Task<ProcessReponseModel<MaterialGroupMaterialModel>> InsertMapping(List<MaterialGroupMaterialModel> data)
        {
            var output = new ProcessReponseModel<MaterialGroupMaterialModel>();
            try
            {
                await _service.InsertMapping(data);
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
        public async Task<ProcessReponseModel<object>> Create([FromBody] MaterialGroupModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Create(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<object>> Update([FromBody] MaterialGroupModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Update(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.Message;
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
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Get")]
        [HttpGet]
        public async Task<ProcessReponseModel<MaterialGroupModel>> Get(int id)
        {
            var output = new ProcessReponseModel<MaterialGroupModel>();
            try
            {
                output.Data =   await _service.Get(id);
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