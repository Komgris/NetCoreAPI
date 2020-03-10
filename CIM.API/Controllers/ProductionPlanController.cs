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
    public class ProductionPlanController : BaseController
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
                    return JsonSerializer.Serialize(result);
                    //return "";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/[controller]/Get/{row}/{pages}")]
        [HttpGet]
        public async Task<PagingModel<ProductionPlanModel>> Get(int row, int pages)
        {
            var model = await _planService.Paging(pages, row);
            return model;
        }

        [Route("api/[controller]/Insert")]
        [HttpPost]
        public bool Insert([FromBody]List<ProductionPlanModel> import)
        {
            try
            {
                _planService.Insert(import);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/[controller]/Update")]
        [HttpPost]
        public bool Update([FromBody]List<ProductionPlanModel> list)
        {
            try
            {
                _planService.Update(list);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/[controller]/Delete/{id}")]
        [HttpDelete]
        public bool Delete(string id)
        {
            try
            {
                _planService.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/[controller]/load")]
        [HttpPost]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Load(ProductionPlanModel model)
        {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _planService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };
                await _planService.Load(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/[controller]/{id}")]
        [HttpGet]
        public async Task<ProcessReponseModel<ProductionPlanModel>> Get(string id)
        {
            var output = new ProcessReponseModel<ProductionPlanModel>();
            try
            {
                output.Data = await _planService.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }
        
    }
}