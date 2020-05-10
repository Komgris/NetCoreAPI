using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MachineTagsService : BaseService, IMachineTagsService
    {
        private IUnitOfWorkCIM _unitOfWork;
        private IMachineTagsRepository _machineTagsRepository;
        public MachineTagsService(
            IUnitOfWorkCIM unitOfWork,
            IMachineTagsRepository machineTagsRepository
            )
        {
            _machineTagsRepository = machineTagsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Get()
        {
            var output = await _machineTagsRepository.Get();
            return JsonConvert.SerializeObject(output);
        }
    }
}
