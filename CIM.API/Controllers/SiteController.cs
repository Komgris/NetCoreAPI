using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            using(var ctx = new cim_dbContext())
            {
                var output = ctx.Sites.Select(x => x.Id).ToArray();
                return output;
            }
        }
    }
}