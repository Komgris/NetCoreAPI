﻿using System;
using System.Collections.Generic;
using System.Text;

using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MachineTypeLossLevel3Service : BaseService, IMachineTypeLossLevel3Service
    {
        private readonly IMachineTypeLossLevel3Repository _machineTypeLossLevel3Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public MachineTypeLossLevel3Service(
            IUnitOfWorkCIM unitOfWork,
            IMachineTypeLossLevel3Repository machineTypeLossLevel3Repository
            )
        {
            _machineTypeLossLevel3Repository = machineTypeLossLevel3Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<MachineTypeLossLevel3ListModel>> List(int? machineTypeId, int? lossLevel3Id, int page, int howmany)
        {
            var output = await _machineTypeLossLevel3Repository.List(machineTypeId, lossLevel3Id, page, howmany);
            return output;
        }
    }
}
