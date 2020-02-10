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
        private IMaterialService _materialService;
        public MaterialController(
            IMaterialService materialService
            )
        {
            _materialService = materialService;
        }
        [HttpGet]
        [Route("api/[controller]/List")]
        public List<MaterialModel> List()
        {
            var output = _materialService.List();
            return output;
        }
        [HttpPost]
        [Route("api/[controller]/Insert")]
        public bool Insert([FromBody]MaterialModel model)
        {
            try
            {
                _materialService.Insert(model);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //same http method like insert, can't this do
        //Add a route like this         [Route("api/[controller]/Compare")]
        [HttpPost]
        [Route("api/[controller]/Update")]
        public bool Update([FromBody]MaterialModel model)
        {
            try
            {
                _materialService.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet]
        [Route("api/[controller]/ListByPaging/{row}/{pages}")]
        public string ListByPaging(int row, int pages)
        {
            var fromDb = _materialService.Paging(pages, row);
            return JsonSerializer.Serialize(fromDb);
        }
        [HttpGet]
        [Route("api/[controller]/Get/{id}")]
        public string Get(int id)
        {
            var fromDb = _materialService.Get(id);
            return JsonSerializer.Serialize(fromDb);
        }
    }
}