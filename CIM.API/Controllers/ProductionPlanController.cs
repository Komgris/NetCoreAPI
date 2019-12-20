using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CIM.Model;
using CIM.BusinessLogic.Interfaces;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Text.Json;

namespace CIM.API.Controllers
{   
    [Route("api/[controller]")]
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
        [HttpGet]
        public string Compare()
        {
            var json = "test";      
            return json;
        }
        //public string Compare()
        //{
        //    var json = "";
        //    var file = Request.Form.Files[0];
        //    if (file.Length > 0)
        //    {
        //        var fromExcel = _planService.ReadImport();
        //        var fromDb = _planService.List();
        //        var result = _planService.Compare(fromExcel, fromDb);
        //        json = JsonSerializer.Serialize(result);
        //    }
        //    return json;
        //}

    }
}