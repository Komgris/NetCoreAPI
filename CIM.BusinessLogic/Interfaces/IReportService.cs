using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.BusinessLogic.Interfaces {
    public interface IReportService : IBaseService {
        DataTable GetProductionSummary(string planId,int routeId,DateTime? from,DateTime? to);
        DataTable GetProductionPlanInfomation(string planId, int routeId);
        DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetCapacityUtilisation(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetProductionOperators(string planId, int routeId); 
        DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? mcId, DateTime? from, DateTime? to);
        DataTable GetProductionDasboard(string planId, int routeId, int mcId);
        Dictionary<string, int> GetActiveProductionPlanOutput();
    }
}
