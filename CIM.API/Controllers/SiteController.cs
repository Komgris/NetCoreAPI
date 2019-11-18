using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic;
using CIM.DAL.Implements;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SiteController : ControllerBase
    {
        private ISiteService _siteService;

        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpGet]
        public List<SiteModel> Get()
        {


            var output = _siteService.List();
            return output;

        }
    }
}