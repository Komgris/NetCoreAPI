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
        [Route("api/[controller]")]
        public async Task<string> Get()
        {
            await _masterDataService.GetData();
            return "OK";

        }

        [HttpGet]
        [Route("api/[controller]/Refresh")]
        public async Task<string> Refresh()
        {
            await _masterDataService.Refresh();
            return "OK";

        }

        [HttpGet]
        [Route("api/[controller]/Clear")]
        public async Task<string> Clear()
        {
            await _masterDataService.Clear();
            return "OK";

        }

    }
}