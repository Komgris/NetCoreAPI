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

        public async Task<LossLevel3Model> Create(LossLevel3EditableModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new LossLevel3());
            _lossLevel3Repository.Add(dbModel);
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }

        public async Task<LossLevel3Model> Update(LossLevel3EditableModel model)
        {
            var dbModel = await _lossLevel3Repository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _lossLevel3Repository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }

        public async Task<PagingModel<Model.LossLevel3ListModel>> List(string keyword, int page, int howmany, bool isActive)
        {
            string sql = @"sp_ListLossLevel3";
            int total;
            Dictionary<string, object> dictParameter = new Dictionary<string, object>
            {
                { "@keyword", keyword },
                { "@is_active", isActive},
                { "@page", page},
                { "@howmany", howmany}
            };

            IList<Domain.Models.LossLevel3ListModel> result;
            result = await _lossLevel3Repository.List(sql, dictParameter);
            var output = new List<Model.LossLevel3ListModel>();
            foreach (var item in result)
            {
                output.Add(MapperHelper.AsModel(item, new Model.LossLevel3ListModel()));
            }

            if (result.Count > 0)
            {
                total = result[0].TotalCount;
            }
            else
            {
                total = 0;
            }

            return new PagingModel<Model.LossLevel3ListModel>
            {
                Total = total,
                Data = output
            };
        }

        public async Task<LossLevel3EditableModel> Get(int id)
        {
            var dbModel = await _lossLevel3Repository.FirstOrDefaultAsync(x => x.Id == id);
            return MapperHelper.AsModel(dbModel, new LossLevel3EditableModel());
        }
    }
}
