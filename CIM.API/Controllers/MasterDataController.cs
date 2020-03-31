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
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private IMasterDataService _masterDataService;

        public MasterDataController(
            IMasterDataService masterDataService 
            )
        {
            _masterDataService = masterDataService;
        }

        [HttpGet]
        public IDictionary<int, MachineComponentModel> Get()
        {

           return _masterDataService.Components;

        }

    }
}