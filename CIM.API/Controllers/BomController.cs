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
        private IResponseCacheService _responseCacheService;
        private IBomService _bomService;
        public BomController(
            IResponseCacheService responseCacheService,
            IBomService bomService
        )
        {
            _responseCacheService = responseCacheService;
            _bomService = bomService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<BomModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<BomModel>>();
            try
            {
                output.Data = await _bomService.List(keyword, page, howmany);
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
    }
}