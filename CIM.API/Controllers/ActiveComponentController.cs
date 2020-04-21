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
    public class ActiveComponentController : BaseController
    {
        private IResponseCacheService _responseCacheService;

        public ActiveComponentController(
            IResponseCacheService responseCacheService
            )
        {
            _responseCacheService = responseCacheService;
        }

    }
}