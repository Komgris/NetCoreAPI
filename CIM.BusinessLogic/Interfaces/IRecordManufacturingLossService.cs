﻿using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordManufacturingLossService : IBaseService
    {
        Task<ActiveProductionPlanModel> Create(RecordManufacturingLossModel model);
        Task<ActiveProductionPlanModel> Update(RecordManufacturingLossModel model);
        Task<RecordManufacturingLossModel> GetByGuid(Guid guid);
        Task<ActiveProductionPlanModel> End(RecordManufacturingLossModel model);
        Task<PagingModel<RecordManufacturingLossModel>> List(string planId, int? routeId, string keyword, int page, int howmany);
        Task<List<RecordManufacturingLossModel>> ListByMonth(int month, int year, string planId, int? routeId = null);
        Task<PagingModel<RecordManufacturingLossModel>> ListByDate(DateTime date, string keyword, int page, int howmany, string planId, int? routeId = null);
        Task<List<RecordManufacturingLossModel>> List3M(string planId, int machineId, bool isAuto, string keyword);
        Task<RecordManufacturingLossModel> GetByGuid3M(Guid guid);
        Task<RecordManufacturingLossModel> Get3M(int id);
        Task<ActiveMachine3MModel> Create3M(RecordManufacturingLossModel model);
        Task<ActiveMachine3MModel> Update3M(RecordManufacturingLossModel model);
        Task Delete3M(int id);
        Task<ActiveMachine3MModel> End3M(RecordManufacturingLossModel model);
        DataTable GetReportLoss3M(string planId, int machineId);
    }
}
