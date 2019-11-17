using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.DAL.Implements;
using CIM.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SiteController : ControllerBase
    {
        public SiteController()
        {

        }
        [HttpGet]
        public int[] Get()
        {


            using (var ctx = new cim_dbContext())
            {
                var site = new SiteRepository(ctx); 
                var output = site.All().Select(x => x.Id).ToArray();
                return output;
            }


        }
    }
}