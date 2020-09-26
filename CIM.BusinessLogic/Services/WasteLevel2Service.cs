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
    public class WasteLevel2Service : BaseService, IWasteLevel2Service
    {
        private readonly IWasteLevel2Repository _wasteLevel2Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public WasteLevel2Service(
            IUnitOfWorkCIM unitOfWork,
            IWasteLevel2Repository wasteLevel2Repository
            )
        {
            _wasteLevel2Repository = wasteLevel2Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WasteLevel2Model> Create(WasteLevel2Model model)
        {
            var dbModel = MapperHelper.AsModel(model, new WasteLevel2());
            _wasteLevel2Repository.Add(dbModel);
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new WasteLevel2Model());
        }

        public async Task<WasteLevel2Model> Update(WasteLevel2Model model)
        {
            WasteLevel2 dbModel = await _wasteLevel2Repository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel.Description = model.Description;
            dbModel.WasteLevel1Id = model.WasteLevel1Id;
            dbModel.IsActive = model.IsActive;
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _wasteLevel2Repository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new WasteLevel2Model());
        }

        public async Task<PagingModel<WasteLevel2Model>> List(string keyword, int page, int howMany, bool isActive, int? processTypeId)
        {
            var output = await _wasteLevel2Repository.ListAsPaging("sp_ListWasteLevel2", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive},
                    {"@processtype", processTypeId}
                }, page, howMany);
            return output;
        }

        public async Task<WasteLevel2Model> Get(int id)
        {
            var dbModel = await _wasteLevel2Repository.FirstOrDefaultAsync(x => x.Id == id);
            return MapperHelper.AsModel(dbModel, new WasteLevel2Model());
        }
    }
}
