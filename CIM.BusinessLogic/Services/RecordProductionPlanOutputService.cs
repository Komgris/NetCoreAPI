using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanOutputService : BaseService, IRecordProductionPlanOutputService
    {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IDirectSqlRepository _directSqlRepository;
        private IMachineService _machineService;
        private IRecordProductionPlanOutputRepository _recordProductionPlanOutputRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanOutputService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IDirectSqlRepository directSqlRepository,
            IMachineService machineService,
            IRecordProductionPlanOutputRepository recordProductionPlanOutputRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _directSqlRepository = directSqlRepository;
            _machineService = machineService;
            _recordProductionPlanOutputRepository = recordProductionPlanOutputRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessReponseModel<bool>> UpdateMachineProduceCounter(List<MachineProduceCounterModel> listData, int hour)
        {

            return null;
        }
    }
}
