using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ComponentTypeLossLevel3Controller : ControllerBase
    {
        private IComponentTypeLossLevel3Service _service;
        public ComponentTypeLossLevel3Controller(
            IComponentTypeLossLevel3Service service
        )
        {
            _service = service;
        }
    }
}