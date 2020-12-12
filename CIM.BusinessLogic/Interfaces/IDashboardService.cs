using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Interfaces {
    public interface IDashboardService : IBaseService {
        DataTable GetProductionSummary(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetProductionPlanInfomation(string planId, int routeId);
        DataTable GetProductionOperators(string planId, int routeId);
        DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetCapacityUtilisation(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByMaterials(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByCases(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByMachines(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteCostByTime(string planId, int routeId, DateTime? from, DateTime? to); 
        DataTable GetActiveMachineInfo(string planId, int routeId);
        DataTable GetActiveMachineEvents(string planId, int routeId);
        DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? mcId, int? lossId, DateTime? from, DateTime? to);
        DataTable GetProductionDasboard(string planId, int routeId, int mcId);
        Task<ProductionDataModel> GenerateBoardcast(DataTypeGroup updateType, string productionPlan, int routeId);
        Task<ProductionDataModel> GenerateBoardcastManagementData(DataFrame timeFrame, BoardcastType updateType);
        Task<ActiveProductionPlanModel> GenerateBoardcastOperationData(DataTypeGroup updateType, string productionPlan, int routeId);
        Task<ProductionDataModel> GetManagementDashboard(DataFrame frame);
        Task<ProductionDataModel> GenerateCustomDashboard(DataTypeGroup updateType);

        DataTable GetChartData(DateTime? datestamp, string chartData, string sourceData);

    }
}
