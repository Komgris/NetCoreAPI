﻿using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IResponseCacheService
    {
        Task SetAsync(string key, object model);
        Task SetAsyncExpire(string key, object model, int ttlSec);
        Task<string> GetAsync(string key);
        Task<T> GetAsTypeAsync<T>(string key);
        Task RemoveAsync(string key);
        Task SetActivePlan(ActiveProductionPlan3MModel model);
        ActiveProductionPlan3MModel GetActivePlan(string planId);
        void RemoveActivePlan(string planId);
        ActiveMachine3MModel GetActiveMachine(int machineId);
        Task SetActiveMachine(ActiveMachine3MModel model);
        ProductionInfoModel GetProductionInfo();
        Task SetProductionInfo(ProductionInfoModel model);
        DataTable GetDashboardData(string chartId);
        Task SetDashboardData(string chartId, DataTable data);
        Task<DataTable> UpdateDashboardData(string chartId, int machineId, Dictionary<string, object> data);
    }
}
