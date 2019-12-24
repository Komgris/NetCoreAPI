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

namespace CIM.API.Controllers
{   
    [EnableCors("_myAllowSpecificOrigins")]
    
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
        private IPlanService _planService;
        public ProductionPlanController(
            IPlanService planService
            )
        {
            _planService = planService;
        }
        [Route("api/[controller]/Compare")]
        [HttpGet]
        //public string Compare()
        //{
        //    var json = "test";      
        //    return json;
        //}
        public string Compare()
        {
            string path = @"D:\PSEC\Dole\Doc\test.xlsx";
            //var file = Request.Form.Files[0];
            //if (file.Length > 0)
            //{
                var fromExcel = _planService.ReadImport(path);
                var fromDb = _planService.List();
                var result = _planService.Compare(fromExcel, fromDb);
               return  JsonSerializer.Serialize(result);
            //}
            //else
            //{
            //    return null;
            //}    
        }
        [EnableCors("_myAllowSpecificOrigins")]
        [Route("api/[controller]/Get/{row}/{pages}")]
        [HttpGet]
        public string Get(int row,int pages)
        {

            return JsonSerializer.Serialize("pass"); ;
        }

}
}