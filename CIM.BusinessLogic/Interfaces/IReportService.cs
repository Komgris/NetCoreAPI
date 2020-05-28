using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Interfaces {
    public interface IReportService : IBaseService {
        DataTable GetProductionSummary(string planId,int routeId,DateTime? from,DateTime? to);
        DataTable GetProductionPlanInfomation(string planId, int routeId);
        DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetCapacityUtilisation(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetProductionOperators(string planId, int routeId); 
        DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? mcId, DateTime? from, DateTime? to);
        PagingModel<object> GetProductionWCMLossHistory(string planId, int routeId, DateTime? from, DateTime? to, int page);
        DataTable GetProductionDasboard(string planId, int routeId, int mcId);
        DataTable GetWasteByMaterials(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByCases(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByMachines(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteCostByTime(string planId, int routeId, DateTime? from, DateTime? to);
        PagingModel<object> GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to, int page);
        DataTable GetActiveMachineInfo(string planId, int routeId); 
        DataTable GetActiveMachineEvents(string planId, int routeId); 
        Dictionary<string, int> GetActiveProductionPlanOutput();
        PagingModel<object> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null);
        BoardcastDataModel GetDashboardData(DashboardConfig dashboardConfig, DashboardTimeFrame type, Dictionary<string, object> paramsList);

    }
}
