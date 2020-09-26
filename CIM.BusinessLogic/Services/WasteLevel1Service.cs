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
    public class WasteLevel1Service : BaseService, IWasteLevel1Service
    {
        private readonly IWasteLevel1Repository _wasteLevel1Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public WasteLevel1Service(
            IUnitOfWorkCIM unitOfWork,
            IWasteLevel1Repository wasteLevel1Repository
            )
        {
            _wasteLevel1Repository = wasteLevel1Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WasteLevel1Model> Create(WasteLevel1Model model)
        {
            var dbModel = MapperHelper.AsModel(model, new WasteLevel1());
            _wasteLevel1Repository.Add(dbModel);
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new WasteLevel1Model());
        }

        public async Task<WasteLevel1Model> Update(WasteLevel1Model model)
        {
            WasteLevel1 dbModel = await _wasteLevel1Repository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel.Description = model.Description;
            dbModel.ProcessTypeId = model.ProcessTypeId;
            dbModel.IsActive = model.IsActive;
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _wasteLevel1Repository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new WasteLevel1Model());
        }

        public async Task<PagingModel<WasteLevel1Model>> List(string keyword, int page, int howMany, bool isActive, int? processTypeId)
        {
            var output = await _wasteLevel1Repository.ListAsPaging("sp_ListWasteLevel1", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive},
                    {"@processtype", processTypeId}
                }, page, howMany);
            return output;
        }

        public async Task<WasteLevel1Model> Get(int id)
        {
            var dbModel = await _wasteLevel1Repository.FirstOrDefaultAsync(x => x.Id == id);
            return MapperHelper.AsModel(dbModel, new WasteLevel1Model());
        }
    }
}
