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
    public class LossLevel3Service : BaseService, ILossLevel3Service
    {
        private readonly ILossLevel3Repository _repository;
        private IUnitOfWorkCIM _unitOfWork;

        public LossLevel3Service(
            IUnitOfWorkCIM unitOfWork,
            ILossLevel3Repository repository
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LossLevel3Model> Create(LossLevel3EditableModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new LossLevel3());
            _repository.Add(dbModel);
            dbModel.Id = 0;
            dbModel.IsActive = true;
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }

        public async Task<LossLevel3Model> Update(LossLevel3EditableModel model)
        {
            var dbModel = await _repository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _repository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new LossLevel3Model());
        }

        public async Task<PagingModel<LossLevel3ViewModel>> List(string keyword, int page, int howmany)
        {
            bool isActive = true;
            var result = await _repository.List(page, howmany, keyword, isActive);
            var output = new List<LossLevel3ViewModel>();
            foreach (var item in result.Data)
            {
                output.Add(MapperHelper.AsModel(item, new LossLevel3ViewModel()));
            }
            return new PagingModel<LossLevel3ViewModel>
            {
                HowMany = result.Total,
                Data = output
            };
        }

        public async Task<LossLevel3EditableModel> Get(int id)
        {
            var result = await _repository.Get(id);
            if (result == null)
            {
                return null;
            }
            var output = MapperHelper.AsModel(result, new LossLevel3EditableModel());
            return output;
        }
    }
}
