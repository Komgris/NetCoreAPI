using System;
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

        public async Task Update(int machineTypeId, List<int> lossLevel3Ids)
        {
            var list = _machineTypeLossLevel3Repository.Where(x => x.MachineTypeId == machineTypeId);
            foreach (var model in list)
            {
                _machineTypeLossLevel3Repository.Delete(model);
            }

            foreach (var lossLevel3Id in lossLevel3Ids)
            {
                var db_model = new MachineTypeLossLevel3();
                db_model.LossLevel3Id = lossLevel3Id;
                db_model.MachineTypeId = machineTypeId;
                _machineTypeLossLevel3Repository.Add(db_model);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task Insert(List<MachineTypeLossLevel3Model> data)
        {
            foreach (var model in data)
            {
                var db_model = new MachineTypeLossLevel3();
                db_model.LossLevel3Id = model.LossLevel3Id;
                db_model.MachineTypeId = model.MachineTypeId;
                _machineTypeLossLevel3Repository.Add(db_model);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteByMachineTypeId(int machineTypeId)
        {
            var list = _machineTypeLossLevel3Repository.Where(x => x.MachineTypeId == machineTypeId);
            foreach (var model in list)
            {
                _machineTypeLossLevel3Repository.Delete(model);
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
