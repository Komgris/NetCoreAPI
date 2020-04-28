﻿using CIM.Model;
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
        PagingModel<object> GetProductionWCMLossHistory(string planId, int routeId, DateTime? from, DateTime? to, int page);
        DataTable GetProductionDasboard(string planId, int routeId, int mcId);
        DataTable GetWasteByMaterials(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByCases(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteByMachines(string planId, int routeId, DateTime? from, DateTime? to);
        DataTable GetWasteCostByTime(string planId, int routeId, DateTime? from, DateTime? to);
        PagingModel<object> GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to, int page);
        Dictionary<string, int> GetActiveProductionPlanOutput();
    }
}
