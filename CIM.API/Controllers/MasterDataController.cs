﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CIM.API.Controllers
{

    [ApiController]
    public class MasterDataController : BaseController
    {
       
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
            var result = await _masterDataService.GetData();
            var stringOut = JsonConvert.SerializeObject( new { data = result }, JsonsSetting);
            return stringOut;

        }

        [HttpGet]
        [Route("api/[controller]/Refresh")]
        public async Task<string> Refresh()
        {
            await _masterDataService.Refresh(Constans.MasterDataType.All);
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