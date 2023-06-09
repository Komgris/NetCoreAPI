﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [ApiController]
    public class MasterDataShowController : BaseNoSecureController
    {
        private IMasterDataShowService _service;
        public MasterDataShowController(
            //IHubContext<GlobalHub> hub,
            IMasterDataShowService service
        )
        {
            //_hub = hub;
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/GetListDefect")]
        public async Task<string> GetListDefect()
        {
            return JsonConvert.SerializeObject(await _service.GetListWaste()); 
        }

        [HttpGet]
        [Route("api/[controller]/GetListMaterial")]
        public async Task<string> GetListMaterial()
        {
            return JsonConvert.SerializeObject(await _service.GetListMaterial());
        }

        [HttpGet]
        [Route("api/[controller]/GetListMachine")]
        public async Task<string> GetListMachine()
        {
            return JsonConvert.SerializeObject(await _service.GetListMachine());
        }

        [HttpGet]
        [Route("api/[controller]/GetListLoss")]
        public async Task<string> GetListLoss()
        {
            return JsonConvert.SerializeObject(await _service.GetListLoss());
        }

        [HttpGet]
        [Route("api/[controller]/GetListProduct")]
        public async Task<string> GetListProduct()
        {
            return JsonConvert.SerializeObject(await _service.GetListProduct());
        }

        [HttpGet]
        [Route("api/[controller]/GetListProduction")]
        public async Task<string> GetListProduction()
        {
            return JsonConvert.SerializeObject(await _service.GetListProduction());
        }


    }


}
