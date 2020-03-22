using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MachineService : BaseService, IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public MachineService(
            IUnitOfWorkCIM unitOfWork,
            IMachineRepository machineRepository
            )
        {
            _machineRepository = machineRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MachineModel> Create(MachineModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Machine());
            _machineRepository.Add(dbModel);
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new MachineModel());
        }

        public async Task<MachineModel> Get(int id)
        {
            var dbModel = await _machineRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsActive && x.IsDelete == false);
            return MapperHelper.AsModel(dbModel, new MachineModel());
        }

        public async Task<PagingModel<MachineModel>> List(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            //to do optimize
            var dbModel = (await _machineRepository.WhereAsync(x => x.IsActive && x.IsDelete == false &
                                (x.Name.Contains(keyword)
                                || x.Plcaddress.Contains(keyword)
                                || x.Status.Name.Contains(keyword)
                                || x.MachineType.Name.Contains(keyword))));

            int total = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();

            var output = new List<MachineModel>();
            foreach (var item in dbModel)
                output.Add(MapperHelper.AsModel(item, new MachineModel()));

            return new PagingModel<MachineModel>
            {
                HowMany = total,
                Data = output
            };
        }

        public async Task<MachineModel> Update(MachineModel model)
        {
            var dbModel = await _machineRepository.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsDelete == false);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _machineRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new MachineModel());
        }
    }
}
