using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CIM.Model;
using CIM.BusinessLogic.Interfaces;

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
        [HttpPost]
        public List<ProductionPlanModel> Compare(List<ProductionPlanModel> import)
        {
            var existing = _planService.List();
            var output = _planService.Compare(import, existing);
            return output;
        }
    }
}