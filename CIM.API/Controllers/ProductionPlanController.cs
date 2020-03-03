using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CIM.Model;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Net.Http.Headers;

namespace CIM.API.Controllers
{   
    [EnableCors("_myAllowSpecificOrigins")]
    
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
        private IProductionPlanService _planService;
        public ProductionPlanController(
            IProductionPlanService planService
            )
        {
            _planService = planService;
        }

        [Route("api/[controller]/Compare")]
        [HttpPost]
        public string Compare()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("ProductionPlan");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                     var fromExcel = _planService.ReadImport(fullPath);
                     var fromDb = _planService.Get();
                     var result = _planService.Compare(fromExcel, fromDb);
                    return  JsonSerializer.Serialize(result);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }  
        }

        [Route("api/[controller]/Get/{row}/{pages}")]
        [HttpGet]
        public string Get(int row,int pages)
        {
            var fromDb = _planService.Paging(pages,row);
            return JsonSerializer.Serialize(fromDb);
        }

        [Route("api/[controller]/GetNoPaging")]
        [HttpGet]
        public string GetNoPaging()
        {
            var result = _planService.Get();
            return JsonSerializer.Serialize(result);
        }

        [Route("api/[controller]/Insert")]
        [HttpPost]
        public async Task<bool> Insert([FromBody]List<ProductionPlanModel> import)
        {
            try
            {
                await  _planService.Insert(import);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/[controller]/Update")]
        [HttpPost]
        public async Task<bool> Update([FromBody]List<ProductionPlanModel> list)
        {
            try
            {
                await _planService.Insert(list);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/[controller]/Delete/{id}")]
        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            try
            {
                await _planService.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}