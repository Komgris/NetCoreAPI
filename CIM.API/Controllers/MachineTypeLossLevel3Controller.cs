using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;

namespace CIM.API.Controllers
{
    [ApiController]
    public class MachineTypeLossLevel3Controller : ControllerBase
    {
        private IMachineTypeLossLevel3Service _service;
        public MachineTypeLossLevel3Controller(
            IMachineTypeLossLevel3Service service
        )
        {
            _service = service;
        }

    }
}