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
            var result = await _masterDataService.GetData();
            var stringOut = JsonConvert.SerializeObject( new { data = result }, _masterDataService.JsonsSetting);
            return stringOut;

        }

        [HttpGet]
        [Route("api/[controller]/Operation")]
        public async Task<string> GetOperation()
        {
            var result = await _masterDataService.GetDataOperation();
            var stringOut = JsonConvert.SerializeObject(new { data = result }, _masterDataService.JsonsSetting);
            return stringOut;

        }

        [HttpGet]
        [Route("api/[controller]/Refresh")]
        public async Task<string> Refresh()
        {
            try
            {
                await _masterDataService.Refresh(Constans.MasterDataType.All);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
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