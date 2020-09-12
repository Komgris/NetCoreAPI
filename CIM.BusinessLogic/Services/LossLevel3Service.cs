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
    public class LossLevel3Service : BaseService, ILossLevel3Service
    {
        private readonly ILossLevel3Repository _lossLevel3Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public LossLevel3Service(
            IUnitOfWorkCIM unitOfWork,
            ILossLevel3Repository lossLevel3Repository
            )
        {
            _lossLevel3Repository = lossLevel3Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LossLevel3Model> Create(LossLevel3Model model)
        {
            var dbModel = MapperHelper.AsModel(model, new LossLevel3());
            _lossLevel3Repository.Add(dbModel);
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }

        public async Task<LossLevel3Model> Update(LossLevel3Model model)
        {
            LossLevel3 dbModel = await _lossLevel3Repository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel.Name = model.Name;
            dbModel.Description = model.Description;
            dbModel.LossLevel2Id = Convert.ToInt16(model.LossLevel2Id);
            dbModel.IsActive = model.IsActive;
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _lossLevel3Repository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }

        public async Task<PagingModel<Model.LossLevel3ListModel>> List(string keyword, int page, int howmany, bool isActive, int? lossLevel2Id)
        {
            var output = await _lossLevel3Repository.List(page, howmany, keyword, isActive, lossLevel2Id);
            return output;
        }

        public async Task<LossLevel3Model> Get(int id)
        {
            var dbModel = await _lossLevel3Repository.FirstOrDefaultAsync(x => x.Id == id);
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }
    }
}
