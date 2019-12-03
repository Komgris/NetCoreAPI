using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectSqlController : ControllerBase
    {
        private IDirectSqlService _directSqlService;

        public DirectSqlController(
            IDirectSqlService directSqlService)
        {
            _directSqlService = directSqlService;
        }

        [HttpGet]
        public string ExecuteReader(string sql, string parameters)
        {
            return _directSqlService.ExecuteReader(sql, null);
        }

        [HttpPost]
        public bool ExecuteNonQuery(string sql, string parameters)
        {
            try
            {
                _directSqlService.ExecuteNonQuery(sql, null);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}