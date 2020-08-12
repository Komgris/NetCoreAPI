using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordMaintenancePlanService : BaseService, IRecordMaintenancePlanService
    {
        private readonly IResponseCacheService _responseCacheService;
        private IUnitOfWorkCIM _unitOfWork;
        private readonly IRecordMaintenancePlanRepository _recordMaintenancePlanRepositiry;

        public RecordMaintenancePlanService(
            IResponseCacheService responseCacheService,
            IUnitOfWorkCIM unitOfWork,
            IRecordMaintenancePlanRepository recordMaintenancePlanRepository
            )
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _recordMaintenancePlanRepositiry = recordMaintenancePlanRepository;
        }

        public async Task Create(RecordMaintenancePlanModel data)
        {
            var db_model = MapperHelper.AsModel(data, new RecordMaintenancePlan());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            _recordMaintenancePlanRepositiry.Add(db_model);
            await _unitOfWork.CommitAsync();

        }

        public async Task<PagingModel<RecordMaintenancePlanModel>> List(string keyword, int page, int howMany)
        {
            var output = await _recordMaintenancePlanRepositiry.ListAsPaging("sp_ListRecordMaintenancePlan", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    { "@page", page}
                }, page, howMany);
            return output;
        }

        public async Task<List<RecordMaintenancePlanModel>> ListByDate(DateTime date)
        {
            var output = await _recordMaintenancePlanRepositiry.List("sp_ListRecordMaintenancePlanByDate", new Dictionary<string, object>()
                {
                    {"@date", date}
                });
            return output;
        }

        public async Task<List<RecordMaintenancePlanModel>> ListByMonth(int month, int year, bool isActive)
        {
            var output = await _recordMaintenancePlanRepositiry.List("sp_ListRecordMaintenancePlanByMonth", new Dictionary<string, object>()
                {
                    {"@month", month},
                    {"@year", year},
                    {"@is_active", isActive}
                });
            return output;
        }

        public async Task Update(RecordMaintenancePlanModel data)
        {
            var db_model = MapperHelper.AsModel(data, new RecordMaintenancePlan());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _recordMaintenancePlanRepositiry.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<RecordMaintenancePlanModel> Get(int id)
        {
            return await _recordMaintenancePlanRepositiry.All().Select(
            x => new RecordMaintenancePlanModel
            {
                Id = x.Id,
                MachineId = x.MachineId,
                TeamId = x.TeamId,
                PlanStart = x.PlanStart,
                ActualStart = x.ActualStart,
                ActualFinish = x.ActualFinish,
                Details = x.Details,
                Note = x.Note,
                ActualCount = x.ActualCount,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy
            }).FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
