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
                var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = currentUser;

               return await _service.Create(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<object> Update([FromBody]MaterialModel model)
        {
            try
            {
                var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = currentUser;

                return await _service.Update(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<PagingModel<MaterialModel>> List(int page = 1, int howmany = 10)
        {
            try
            {
                var result = await _service.List(page, howmany);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<MaterialModel> Get(int id)
        {
            try
            {
                var result = await _service.Get(id);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}