using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ComponentTypeLossLevel3Controller : ControllerBase
    {
        private IComponentTypeLossLevel3Service _service;
        public ComponentTypeLossLevel3Controller(
            IComponentTypeLossLevel3Service service
        )
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<ComponentTypeLossLevel3ListModel>>> List(int id = 0, string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<ComponentTypeLossLevel3ListModel>>();
            try
            {
                //output.Data = await _service.List(keyword, page, howmany, isActive);
                //output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }
    }
}