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
    public class BomController : BaseController
    {
        private IBomService _bomService;
        public BomController(
            IBomService bomService
        )
        {
            _bomService = bomService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<BomModel>>> List(string keyword = "", int page = 1, int howmany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<BomModel>>();
            try
            {
                output.Data = await _bomService.List(keyword, page, howmany, isActive);
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
        public async Task<ProcessReponseModel<List<BomMaterialModel>>> ListMaterial(int bomId)
        {
            var output = new ProcessReponseModel<List<BomMaterialModel>>();
            try
            {
                output.Data = await _bomService.ListBomMapping(bomId);
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
        public async Task<ProcessReponseModel<BomMaterialModel>> InsertMapping(List<BomMaterialModel> data)
        {
            var output = new ProcessReponseModel<BomMaterialModel>();
            try
            {
                await _bomService.InsertMapping(data);
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
        public async Task<ProcessReponseModel<object>> Create([FromBody] BomModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _bomService.Create(data);
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
        public async Task<ProcessReponseModel<object>> Update([FromBody] BomModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _bomService.Update(data);
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
                await _bomService.Delete(id);
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
        public async Task<ProcessReponseModel<BomModel>> Get(int Id)
        {
            var output = new ProcessReponseModel<BomModel>();
            try
            {
                output.Data =  _bomService.Get(Id);
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