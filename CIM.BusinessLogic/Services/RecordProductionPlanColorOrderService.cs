using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanColorOrderService : BaseService, IRecordProductionPlanColorOrderService
    {
        private IRecordProductionPlanColorOrderRepository _recordProductionPlanColorOrderRepository;
        private IRecordProductionPlanColorOrderDetailRepository _recordProductionPlanColorOrderDetailRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanColorOrderService(
            IRecordProductionPlanColorOrderRepository recordProductionPlanColorOrderRepository,
            IRecordProductionPlanColorOrderDetailRepository recordProductionPlanColorOrderDetailRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _recordProductionPlanColorOrderRepository = recordProductionPlanColorOrderRepository;
            _recordProductionPlanColorOrderDetailRepository = recordProductionPlanColorOrderDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RecordProductionPlanColorOrderModel> Compare(RecordProductionPlanColorOrderModel model)
        {
            var recordColor = (await _recordProductionPlanColorOrderRepository.FirstOrDefaultAsync(x => x.ProductionPlanId == model.ProductionPlanId));
            if (recordColor == null)
            {
                return await Create(model);
            }
            else
            {
                return await Update(model);
            }
        }

        public async Task<RecordProductionPlanColorOrderModel> Update(RecordProductionPlanColorOrderModel model)
        {
                var dbModel = await _recordProductionPlanColorOrderRepository.Get(model.Id);
                int order = 1;
                foreach (var item in dbModel.RecordProductionPlanColorOrderDetail)
                {
                    _recordProductionPlanColorOrderDetailRepository.Delete(item);
                }
                dbModel.RecordProductionPlanColorOrderDetail.Clear();
                foreach (var datails in model.Colordetail)
                {
                    var colorList = MapperHelper.AsModel(datails, new RecordProductionPlanColorOrderDetail());
                    colorList.Sequence = order;
                    dbModel.RecordProductionPlanColorOrderDetail.Add(colorList);
                    order++;
            }

                if (dbModel.RecordProductionPlanColorOrderDetail.Count > 0)
                {
                    dbModel.UpdatedAt = DateTime.Now;
                    dbModel.UpdatedBy = CurrentUser.UserId;
                    _recordProductionPlanColorOrderRepository.Edit(dbModel);
                }
                await _unitOfWork.CommitAsync();
                return model;
        }

        public async Task<RecordProductionPlanColorOrderModel> Create(RecordProductionPlanColorOrderModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanColorOrder());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;
            int order = 1;
            foreach (var datails in model.Colordetail)
            {
                var colorList = MapperHelper.AsModel(datails, new RecordProductionPlanColorOrderDetail());
                colorList.Sequence = order;
                dbModel.RecordProductionPlanColorOrderDetail.Add(colorList);
                order++;
            }

            if (dbModel.RecordProductionPlanColorOrderDetail.Count > 0)
            {
                _recordProductionPlanColorOrderRepository.Add(dbModel);
            }
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<List<RecordProductionPlanColorOrderModel>> List(string planId)
        {
            var output = await _recordProductionPlanColorOrderRepository.List("sp_ListRecordColor", new Dictionary<string, object>()
                {
                    {"@plan_id", planId}
                });
            return output;
        }
    }
}
