using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic;
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Implements;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public object Register(RegisterUserModel model)
        {
            try
            {
                _service.Register(model);
                return new object();
            }
            catch (Exception e)
            {
                return e;
            }

        }
    }
}